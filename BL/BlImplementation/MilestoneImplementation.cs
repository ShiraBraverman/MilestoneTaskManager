using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.ComponentModel.Design.Serialization;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = Factory.Get;

    //public void RemoveRedundantDependencies(List<Dependency> dependencies, int currentDependentTask, int initialDependentTask, int currentDependsOnTask)
    //{
    //    // בדוק עבור תלות ישירות
    //    var directDependencies = dependencies.Where(dep => dep.DependentTask == currentDependentTask && dep.DependsOnTask != initialDependentTask);

    //    foreach (var directDependency in directDependencies)
    //    {
    //        // בדוק אם יש כבר תלות ישירות כמו תלות עקיפה
    //        var existingDirectDependency = dependencies.FirstOrDefault(dep => dep.DependentTask == currentDependentTask && dep.DependsOnTask == directDependency.DependsOnTask);

    //        // אם קיימת כבר תלות ישירה, מחק את התלות היישירות המיותרות
    //        if (existingDirectDependency != null)
    //        {
    //            dependencies.Remove(directDependency);
    //        }

    //        // המשך לבדוק רקורסיבית עבור התלות הבאות
    //        RemoveRedundantDependencies(dependencies, directDependency.DependsOnTask, initialDependentTask, directDependency.DependsOnTask);
    //    }
    //}

   // פונקציה Create: יצירת אבני דרך במערכת, המתחילה ונגמרת עם משימות יחודיות
    public IEnumerable<DO.Dependency> Create()
    {
        // קביעת תלות בין המשימות במערכת
        var groupedDependencies = _dal.Dependency.ReadAll()
            .GroupBy(d => d.DependentTask)
            .OrderBy(group => group.Key)
            .Select(group => (group.Key, group.Select(d => d!.DependsOnTask).ToList()))
            .ToList();

        // בחירת רשימות יחודיות
        var uniqueLists = groupedDependencies
     .Select(group => group.Item2.ToList())
     .DistinctBy(list => string.Join(",", list))
     .ToList();

        int milestoneAlias = 1;

        List<DO.Dependency> dependencies = new List<DO.Dependency>();

        // יצירת אבני דרך ותלות בהם
        foreach (var tasksList in uniqueLists)
        {
            if (tasksList != null)
            {
                DO.Task doTask = new DO.Task
                    (0,
                    $"a milestone with Id: {milestoneAlias}",
                    $"M{milestoneAlias}",
                    true,
                    DateTime.Now);
                try
                {
                    int milestoneId = _dal.Task.Create(doTask);

                    foreach (var taskId in tasksList)
                    {
                        dependencies.Add(new DO.Dependency
                        {
                            DependentTask = milestoneId,
                            DependsOnTask = taskId
                        });
                    }

                    foreach (var dependencyGroup in groupedDependencies)
                    {
                        if (dependencyGroup.Item2.SequenceEqual(tasksList))
                        {
                            dependencies.Add(new DO.Dependency
                            {
                                DependentTask = dependencyGroup.Item1,
                                DependsOnTask = milestoneId
                            });
                        }
                    }

                    milestoneAlias++;
                }
                catch (DO.DalAlreadyExistsException ex)
                {
                    throw new BO.BlFailedToCreate($"failed to create Milestone with Alias = M{milestoneAlias}", ex);
                }
            }
        }

        //משימות שלא תלויות בשום משימה
        var independentOnTasks = _dal.Task.ReadAll(t => !t.Milestone)
    .Where(task => !dependencies.Any(d => d.DependentTask == task!.Id))
    .Select(task => task!.Id)
    .ToList();

        DO.Task startMilestone = new DO.Task
               (0,
               $"a milestone with Id: {0}",
               $"Start",
               true,
               DateTime.Now);

        //משימות ששום משימה לא תלויה בהן
        var independentTasks = _dal.Task.ReadAll(t => !t.Milestone)
    .Where(task => !dependencies.Any(d => d.DependsOnTask == task!.Id))
    .Select(task => task!.Id)
    .ToList();

        DO.Task endMilestone = new DO.Task
               (0,
               $"a milestone with Id: {milestoneAlias}",
               $"End",
               true,
               DateTime.Now);

        //מחיקת כל התלויות הקודמות
        _dal.Dependency.ReadAll().ToList().ForEach(d => _dal.Dependency.Delete(d!.Id));

        try
        {
            // יצירת אבני הדרך
            int startMilestoneId = _dal.Task.Create(startMilestone);
            int endMilestoneId = _dal.Task.Create(endMilestone);

            foreach (var task in independentOnTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = task,
                    DependsOnTask = startMilestoneId
                });
            }

            foreach (var task in independentTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = endMilestoneId,
                    DependsOnTask = task
                });
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlFailedToCreate("Failed to create END or START milestone", ex);
        }

        foreach (var dependency in dependencies.ToList())
        {
            if (dependency != null)
                _dal.Dependency.Create(dependency);
        }

        // חישוב התאריכים של כל המשימות והאבני דרך
        CalculateDatesOfTasksAndMilestones();

        return _dal.Dependency.ReadAll()!;
    }

    // פונקציה Read: קריאה של אבן דרך מהמסד הנתונים לפי המזהה שלו
    public Milestone? Read(int id)
    {
        try
        {
            // קריאת אבן הדרך מהמסד הנתונים
            DO.Task? doTaskMilestone = _dal.Task.Read(t => t.Id == id && t.Milestone);
            if (doTaskMilestone == null)
                throw new BO.BlDoesNotExistException($"Milstone with ID={id} does Not exist");

            // קבלת רשימת המשימות שתלויות באבן דרך
            var tasksId = _dal.Dependency.ReadAll(d => d.DependentTask == doTaskMilestone.Id)
                                         .Select(d => d.DependsOnTask);
            var tasks = _dal.Task.ReadAll(t => tasksId.Contains(t.Id)).ToList();

            // יצירת רשימת משימות ממופה
            var tasksInList = tasks.Select(t => new BO.TaskInList
            {
                Id = t.Id,
                Description = t.Description,
                Alias = t.Alias,
                Status = Tools.CalculateStatus(t.Start, t.ForecastDate, t.Deadline, t.Complete)
            }).ToList();

            // חישוב אחוז השלמות של אבן הדרך
            double CompletionPercentage = 0;
            if (tasksInList.Count > 0)
            {
                CompletionPercentage = (tasksInList.Count(t => t.Status == Status.OnTrack) / tasksInList.Count * 0.1) * 100;
            }

            // בניית אובייקט אבן דרך ממופה
            return new BO.Milestone()
            {
                Id = doTaskMilestone.Id,
                Description = doTaskMilestone.Description,
                Alias = doTaskMilestone.Alias,
                CreateAt = doTaskMilestone.CreateAt,
                Status = Tools.CalculateStatus(doTaskMilestone.Start, doTaskMilestone.ForecastDate, doTaskMilestone.Deadline, doTaskMilestone.Complete),
                ForecastDate = doTaskMilestone.ForecastDate,
                Deadline = doTaskMilestone.Deadline,
                Complete = doTaskMilestone.Complete,
                CompletionPercentage = CompletionPercentage,
                Remarks = doTaskMilestone.Remarks,
                Dependencies = tasksInList!
            };
        }
        catch (Exception ex)
        {
            throw new BlFailedToRead("Failed to build milestone ", ex);
        }
    }

    // פונקציה Update: עדכון של מיילסטון במסד הנתונים
    public void Update(BO.Milestone item)
    {
        // ולידציה של מזהה האבן דרך
        Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));
        // ולידציה של שם האבן דרך
        Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

        try
        {
            // קריאת אבן בדרך הקיים מהמסד
            DO.Task oldDoTask = _dal.Task.Read(t => t.Id == item.Id)!;
            // בנייה של משימה חדשה לפי המודל המקודם והמודל החדש
            DO.Task doTask = new DO.Task(item.Id, item.Description, item.Alias, true, item.CreateAt, null, null,null, oldDoTask.Start, item.ForecastDate, item.Deadline, item.Complete, oldDoTask.Deliverables, item.Remarks, null);
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Milsetone with ID={item.Id} not exists", ex);
        }
    }

    private void CalculateDatesOfTasksAndMilestones()
    {
        var lastMilestone = _dal.Task.Read(t => t.Alias == "End");

        if (lastMilestone == null)
            return;

        recursionDatesForMilstones(lastMilestone);
    }

    private void recursionDatesForMilstones(DO.Task milestone)
    {
        // אם כבר יש תאריך סיום לאבן הדרך, יש לסיים
        if (milestone.Deadline != null) return;

        // בדיקת התלות של האבן דרך הנוכחית
        var dependenciesForCheck = _dal.Dependency.ReadAll(d => d.DependsOnTask == milestone.Id);
        if (dependenciesForCheck != null)
        {
            // קבלת מזהי תלות
            var dependenciesIds = dependenciesForCheck.Select(d => d.DependentTask).ToList();
            // קבלת משימות התלות
            var dependentsTask = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
            // אם יש משימה כזו עם תאריך סיום ריק, יש לסיים
            foreach (var task in dependentsTask)
                if (task.Deadline == null) return;
        }


        // חישוב תאריך הסיום האחרון
        DateTime? date = CalculateLatestFinishDate(milestone);

        // קבלת תלות של אבן הדרך הנוכחית
        var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

        if (dependencies == null)
            return;

        // קבלת משימות המתלוות לאבן הדרך
        var dependentTasks = dependencies.Select(d => _dal.Task.Read(t => t.Id == d.DependsOnTask));

        foreach (var task in dependentTasks)
        {
            // עדכון של משימה 
            _dal.Task.Update(
                new DO.Task(
                    task!.Id,
                    task.Description,
                    task.Alias,
                    task.Milestone,
                    task.CreateAt,
                    task.RequiredEffortTime,
                    task.Level,
                    task.Role,
                    task.Start,
                    (DateTime)(date!) - (TimeSpan)(task.RequiredEffortTime!),
                    date,
                    task.Complete,
                    task.Deliverables,
                    task.Remarks,
                    task.EngineerId));

            // קריאה נוספת לפונקציה עבור משימה תלולה
            recursionDatesForMilstones(_dal.Task.Read(t => t.Id == _dal.Dependency.Read(d => d.DependentTask == task.Id)!.DependsOnTask)!);
        }

        // עדכון של אבן הדרך עצמה
        _dal.Task.Update(new DO.Task(
            milestone.Id,
            milestone.Description,
            milestone.Alias,
            true,
            milestone.CreateAt,
            null,
            null,
            null,
            CalculateEarliestStartDate(milestone),
            date,
            null,
            null,
            null,
            null));
    }

    private DateTime? CalculateLatestFinishDate(DO.Task milestone)
    {
        // קבלת כל התלות של אבן הדרך
        var dependencies = _dal.Dependency.ReadAll(d => d.DependsOnTask == milestone.Id);

        // אם אין תלות, התאריך האחרון האפשרי הוא תאריך הסיום המתוכנן של הפרויקט
        if (dependencies == null || dependencies.Count() == 0)
            return _dal.deadlineProject;

        // קביעת תאריך הסיום האחרון האפשרי
        var dependenciesIds = dependencies.Select(d => d.DependentTask).ToList();
        var dependentTasks = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
        DateTime? latestFinishDate = dependentTasks.Max(t =>
        {
            return (DateTime)(t.Deadline) - (TimeSpan)(t.RequiredEffortTime);
        });
        return latestFinishDate;
    }

    private DateTime? CalculateEarliestStartDate(DO.Task milestone)
    {
        // קבלת כל התלות של האבן דרך 
        var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

        // אם אין תלות, התאריך הראשון האפשרי הוא תאריך ההתחלה המתוכנן של הפרויקט
        if (dependencies == null || dependencies.Count() == 0)
            return _dal.startProject;

        // קביעת תאריך התחלה הראשון האפשרי
        var dependenciesIds = dependencies.Select(d => d.DependsOnTask).ToList();
        var dependentTasks = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
        DateTime? earliestStartDate = dependentTasks.Min(t =>
        {
            return t.ForecastDate;
        });
        return earliestStartDate;
    }
}