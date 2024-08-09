using BlApi;
using System.Collections.Generic;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    // קבלת אובייקט IDal מהמפעיל Factory
    private DalApi.IDal _dal = Factory.Get;

    public int Create(BO.Task item)
    {
        // בדיקת תקינות למחרוזת שאינה ריקה
        Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

        // יצירת אובייקט DO.Task מתוך ה-BO שהתקבל
        try
        {
            DO.Task doTask = new DO.Task(
                item.Id, item.Description,
                item.Alias,
                false,
                item.CreateAt,
                item.RequiredEffortTime,
                item.Level != null ? (DO.EngineerExperience)item.Level : null,
                item.Role != null ? (DO.Roles)item.Role : null,
                item.Start,
                item.ForecastDate,
                item.Deadline,
                item.Complete,
                item.Deliverables,
                item.Remarks,
                (item.Engineer != null ? item.Engineer.Id : null));

            // יצירת רשימת תלות על פי התלות שהוגדרו ב-BO
            var dependenciesToCreate = item.Dependencies != null ? item.Dependencies
                .Select(task => new DO.Dependency
                {
                    DependentTask = item.Id,
                    DependsOnTask = task.Id
                })
                .ToList() : (List<DO.Dependency>?)null;
            // יצירת כל תלות המשימה באמצעות ה-Dependency ב-DAL
            if (dependenciesToCreate != null)
            {
                dependenciesToCreate.ForEach(dependency => _dal.Dependency.Create(dependency));
            }

            // יצירת המשימה ב-DAL והחזרת המזהה החדש שנוצר
            int newId = _dal.Task.Create(doTask);

            return newId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            // אם יש שגיאה שמציינת שהמשימה כבר קיימת, החזרת שגיאה עם המזהה שלה
            throw new BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
    }

    // פונקציה Delete: מחיקת משימה ממסד הנתונים לפי מזהה
    public void Delete(int id)
    {
        try
        {
            // מחיקת המשימה ממסד הנתונים לפי מזהה
            _dal.Task.Delete(id);
        }
        catch (Exception ex)
        {
            // אם יש שגיאה במחיקת המשימה, החזרת שגיאה
            throw new BO.BlFailedToDelete($"Failed to delete task with ID={id}", ex);
        }
    }

    // פונקציה Read: קריאת משימה ממסד הנתונים לפי מזהה
    public BO.Task? Read(int id)
    {
        // קריאת משימה ממסד הנתונים לפי מזהה
        DO.Task? doTask = _dal.Task.Read(t => t.Id == id);

        // בדיקה האם המשימה קיימת
        if (doTask == null)
            throw new BlDoesNotExistException($"Task with ID={id} does Not exist");

        // יצירת רשימת משימות ומשימת יעד
        List<TaskInList> tasksList = null;
        MilestoneInTask milestone = null;

        // בדיקה האם המשימה חלק ממשימת יעד
        DO.Dependency checkMilestone = _dal.Dependency.Read(d => d.DependsOnTask == doTask.Id);
        if (checkMilestone != null)
        {
            // יצירת משימת יעד כאשר ישנה תלות
            int milestoneId = checkMilestone.DependentTask;
            DO.Task? milestoneAsATask = _dal.Task.Read(t => t.Id == milestoneId && t.Milestone);
            if (milestoneAsATask != null)
            {
                string aliasOfMilestone = milestoneAsATask.Alias;
                milestone = new MilestoneInTask()
                {
                    Id = milestoneId,
                    Alias = aliasOfMilestone
                };
            }
            else
            {
                tasksList = Tools.CalculateList(id);
            }
        }
        else
        {
            tasksList = Tools.CalculateList(id);
        }

        // קריאה למהנדס של המשימה
        var engineer = _dal.Engineer.Read(e => e.Id == doTask.EngineerId);
        EngineerInTask? engineerInTask = null;
        if (engineer != null)
        {
            engineerInTask = new EngineerInTask()
            {
                Id = engineer.Id,
                Name = engineer.Name
            };
        }

        // המרת רמת הניסיון של המהנדס
        EngineerExperience? level = null;
        if (doTask.Level != null)
        {
            level = (EngineerExperience)doTask.Level;
        }

        // המרת תפקיד המהנדס
        Roles? Role = null;
        if (doTask.Role != null)
        {
            Role = (Roles)doTask.Role;
        }

        // החזרת אובייקט מסוג BO.Task עם המידע המובנה
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            Milestone = milestone,
            CreateAt = doTask.CreateAt,
            RequiredEffortTime = doTask.RequiredEffortTime,
            Level = level,
            Role = Role,
            Start = doTask.Start,
            ForecastDate = doTask.ForecastDate,
            Deadline = doTask.Deadline,
            Complete = doTask.Complete,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Dependencies = tasksList!,
            Engineer = engineerInTask,
            Status = Tools.CalculateStatus(doTask.Start, doTask.ForecastDate, doTask.Deadline, doTask.Complete),
        };
    }

    // פונקציה ReadAll: קריאת כל המשימות ממסד הנתונים
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        // בדיקת פילטר, אם אין פילטר יותר נפשט ונקבל את כל המשימות
        Func<BO.Task, bool>? filter1 = filter != null ? filter! : item => true;

        // יצירת רשימה להכנסת המשימות שנקראו
        List<BO.Task>? boTasks = new List<BO.Task>();

        // לולאה שעוברת על כל המשימות במסד הנתונים
        foreach (DO.Task? doTask in _dal.Task.ReadAll())
        {
            // יצירת רשימת משימות ומשימת יעד
            List<TaskInList>? tasksList = new List<TaskInList>();
            MilestoneInTask? milestone = null;

            // בדיקה האם המשימה חלק ממשימת יעד
            DO.Dependency checkMilestone = _dal.Dependency.Read(d => d.DependsOnTask == doTask.Id);
            if (checkMilestone != null)
            {
                // יצירת משימת יעד כאשר ישנה תלות
                int milestoneId = checkMilestone.DependentTask;
                DO.Task? milestoneAsATask = _dal.Task.Read(t => t.Id == milestoneId && t.Milestone);
                if (milestoneAsATask != null)
                {
                    string aliasOfMilestone = milestoneAsATask.Alias;
                    milestone = new MilestoneInTask()
                    {
                        Id = milestoneId,
                        Alias = aliasOfMilestone
                    };
                }
                else
                {
                    tasksList = Tools.CalculateList(doTask.Id);
                }
            }
            else
            {
                tasksList = Tools.CalculateList(doTask.Id);
            }

            // קריאה למהנדס של המשימה
            var engineer = _dal.Engineer.Read(e => e.Id == doTask.EngineerId);
            EngineerInTask? engineerInTask = null;
            if (engineer != null)
            {
                engineerInTask = new EngineerInTask()
                {
                    Id = engineer.Id,
                    Name = engineer.Name
                };
            }

            // המרת רמת הניסיון של המהנדס
            EngineerExperience? level = null;
            if (doTask!.Level != null)
            {
                level = (EngineerExperience)doTask.Level;
            }

            // המרת תפקיד המהנדס
            Roles? Role = null;
            if (doTask.Role != null)
            {
                Role = (Roles)doTask.Role;
            }

            // הוספת המשימה לרשימה שתוחזר
            boTasks.Add(new BO.Task()
            {
                Id = doTask!.Id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                Milestone = milestone,
                RequiredEffortTime = doTask.RequiredEffortTime,
                Level = level,
                Role = Role,
                CreateAt = doTask.CreateAt,
                Start = doTask.Start,
                ForecastDate = doTask.ForecastDate,
                Deadline = doTask.Deadline,
                Complete = doTask.Complete,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Dependencies = tasksList,
                Engineer = engineerInTask,
                Status = Tools.CalculateStatus(doTask.Start, doTask.ForecastDate, doTask.Deadline, doTask.Complete),
            });
        }

        // החזרת הרשימה של המשימות עם אפשרות לפילטור
        return boTasks.Where(filter1).ToList();
    }

    // פונקציה Update: עדכון משימה במסד הנתונים
    public void Update(BO.Task item)
    {
        // בדיקות תקינות
        Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));
        Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

        try
        {
            // אם המשימה אינה חלק ממשימת יעד, נמחקות כל התלות הקיימות
            if (item.Milestone == null)
            {
                foreach (var item1 in _dal.Dependency.ReadAll(d => d.DependentTask == item.Id))
                {
                    _dal.Dependency.Delete(item1.Id);
                }

                // יצירת תלות חדשות על פי התלות שהוגדרו ב-BO
                if (item.Dependencies != null)
                {
                    foreach (TaskInList doDependency in item.Dependencies)
                    {
                        DO.Dependency doDepend = new DO.Dependency(0, item.Id, doDependency.Id);
                        int idDependency = _dal.Dependency.Create(doDepend);
                    }
                }
            }

            // יצירת אובייקט DO.Task מתוך ה-BO שהתקבל
            DO.Task doTask = new DO.Task(
               item.Id, 
               item.Description,
               item.Alias,
               false,
               item.CreateAt,
               item.RequiredEffortTime,
               item.Level != null ? (DO.EngineerExperience)item.Level : null,
               item.Role != null ? (DO.Roles)item.Role : null,
               item.Start,
               item.ForecastDate,
               item.Deadline,
               item.Complete,
               item.Deliverables,
               item.Remarks,
               (item.Engineer != null ? item.Engineer.Id : null));

            // עדכון המשימה במסד הנתונים
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            // אם יש שגיאה שמציינת שהמשימה כבר קיימת, החזרת שגיאה עם המזהה שלה
            throw new BlAlreadyExistsException($"Engineer with ID={item.Id} not exists", ex);
        }
    }
}