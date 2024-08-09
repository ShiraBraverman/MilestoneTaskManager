using BO;
using DalTest;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        enum MainMenu { EXIT, MILESTONE, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        public static void MilestoneMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, ,UPDATE }");//milestone has 4 options to choose
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out chooseSubMenu);//using tryparse function

                switch (chooseSubMenu)
                {
                    case 1://create
                        try
                        {
                            s_bl.Milestone.Create();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 2://read milstone
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out id);
                        try
                        {
                            if (s_bl.Milestone!.Read(id) is null)
                                Console.WriteLine("no milestone's task found");
                            else
                            {
                                Console.WriteLine(s_bl.Milestone!.Read(id)!.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 3://update milstone
                        int idMilestoneUpdate;
                        string milestoneDescriptionUpdate,
                            milestoneAliasUpdate;
                        string? milestoneRemarksUpdate;
                        Console.WriteLine("Enter id for reading milestone");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out idMilestoneUpdate);
                        try
                        {
                            Milestone updatedMilestone = s_bl.Milestone.Read(idMilestoneUpdate)!;
                            Console.WriteLine(updatedMilestone.ToString());
                            Console.WriteLine("Enter description, alias, remarks ");//if null to put the same details
                            milestoneDescriptionUpdate = Console.ReadLine();
                            if (milestoneDescriptionUpdate == null || milestoneDescriptionUpdate == "")
                            { milestoneDescriptionUpdate = updatedMilestone.Description; }
                            milestoneAliasUpdate = Console.ReadLine() ;
                            if (milestoneAliasUpdate == null || milestoneAliasUpdate == "") 
                            { milestoneAliasUpdate = updatedMilestone.Alias; }
                            milestoneRemarksUpdate = Console.ReadLine();
                            if (milestoneRemarksUpdate == null || milestoneRemarksUpdate == "")
                            { milestoneRemarksUpdate = updatedMilestone.Remarks; }
                            BO.Milestone newMilUpdate = new BO.Milestone()
                            {
                                Id = idMilestoneUpdate,
                                Description = milestoneDescriptionUpdate,
                                Alias = milestoneAliasUpdate,
                                CreateAt = s_bl.Milestone.Read(idMilestoneUpdate)!.CreateAt,
                                Status = s_bl.Milestone.Read(idMilestoneUpdate)!.Status,
                                ForecastDate = s_bl.Milestone.Read(idMilestoneUpdate)!.ForecastDate,
                                Deadline = s_bl.Milestone.Read(idMilestoneUpdate)!.Deadline,
                                Complete = s_bl.Milestone.Read(idMilestoneUpdate)!.Complete,
                                CompletionPercentage = s_bl.Milestone.Read(idMilestoneUpdate)!.CompletionPercentage,
                                Remarks = milestoneRemarksUpdate,
                                Dependencies = s_bl.Milestone.Read(idMilestoneUpdate)!.Dependencies
                            };
                            s_bl.Milestone.Update(newMilUpdate);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    default: break;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu <= 3);

        }
        public static void EngineerMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1://create engineer
                        Console.WriteLine("Enter id, name,isactive, email, level, cost and role");
                        int idEngineer,
                            idTask;
                        string nameEngineer,
                               emailEngineer,
                               inputEE,
                               inputR; ;
                        DO.EngineerExperience levelEngineer;
                        DO.Roles role;
                        double costEngineer;
                        try
                        {
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idEngineer);
                            nameEngineer = (Console.ReadLine()!);
                            emailEngineer = Console.ReadLine()!;
                            inputEE = Console.ReadLine()!;
                            inputR = Console.ReadLine()!;
                            levelEngineer = (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), inputEE);
                            double.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a doublenumber please"), out costEngineer);
                            role = (DO.Roles)Enum.Parse(typeof(DO.Roles), inputR);
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idTask);
                            BO.Engineer newEng = new BO.Engineer()
                            {
                                Id = idEngineer,
                                Name = nameEngineer,
                                Email = emailEngineer,
                                Level = (BO.EngineerExperience)levelEngineer,
                                Cost = costEngineer,
                                Role = (BO.Roles)role,
                                Task = new BO.TaskInEngineer()
                                {
                                    Id = idTask,
                                    Description = s_bl.Task.Read(idTask)!.Description
                                }
                            };
                            s_bl.Engineer.Create(newEng);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 2://read engineer
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out id);
                        try
                        {
                            if (s_bl.Engineer!.Read(id) is null)
                                Console.WriteLine("no engineer found");
                            else
                            {
                                Console.WriteLine(s_bl.Engineer!.Read(id)!.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                        break;
                    case 3://read all engineer
                        try
                        {
                            s_bl.Engineer!.ReadAll()
                            .ToList()
                            .ForEach(engineer => Console.WriteLine(engineer.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 4://update engineer
                        int idEngineerUpdate,
                            idTaskUpdate;
                        string nameEngineerUpdate,
                            emailEngineerUpdate;
                        string inputUpdate;
                        BO.Roles roleUpdate;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine()!);
                        try
                        {
                            Engineer updatedEngineer = s_bl.Engineer.Read(idEngineerUpdate)!;
                            Console.WriteLine(updatedEngineer.ToString());
                            Console.WriteLine("Enter name");//if null to put the same details
                            nameEngineerUpdate = Console.ReadLine();
                            if (nameEngineerUpdate == null || nameEngineerUpdate == "")
                            {
                                nameEngineerUpdate = updatedEngineer.Name!;
                            }
                            Console.WriteLine("email");
                            emailEngineerUpdate = Console.ReadLine() ?? updatedEngineer.Email;
                            if (emailEngineerUpdate == null || emailEngineerUpdate == "")
                            {
                                emailEngineerUpdate = updatedEngineer.Email;
                            }
                            Console.WriteLine("1-5 to level");
                            inputUpdate = Console.ReadLine()!;
                            levelEngineerUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedEngineer.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdate);
                            
                            Console.WriteLine("1-3 to role");
                            inputUpdate = Console.ReadLine()!;
                            roleUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedEngineer.Role : (BO.Roles)Enum.Parse(typeof(BO.Roles), inputUpdate);


                            Console.WriteLine("cost");
                            inputUpdate = Console.ReadLine()!;
                            costEngineerUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedEngineer.Cost : int.Parse(inputUpdate);

                            BO.Engineer newEngUpdate = new BO.Engineer()
                            {
                                Id = idEngineerUpdate,
                                Name = nameEngineerUpdate,
                                Email = emailEngineerUpdate,
                                Level = (BO.EngineerExperience)levelEngineerUpdate,
                                Cost = costEngineerUpdate,
                                Role = roleUpdate,
                                Task = updatedEngineer.Task
                            };
                            try
                            {
                                s_bl.Engineer.Update(newEngUpdate);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 5://delete engineer
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                        try
                        {
                            s_bl.Engineer!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                                    Console.WriteLine(ex);
                        }

                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        private static void TaskMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out chooseSubMenu);
                
                switch (chooseSubMenu)
                {
                    case 1://create task
                        int days,
                            engineerInTaskId,
                            taskInListId;
                        string taskDescription,
                               taskAlias,
                               taskDeliverables,
                               taskRemarks,
                               inputEE;
                        TimeSpan requiredEffortTime;
                        EngineerExperience taskLevel;
                        List<BO.TaskInList?> taskInList = new List<TaskInList?>();
                        Console.WriteLine("Enter  description");
                        taskDescription = Console.ReadLine()!;
                        Console.WriteLine("Enter  alias");
                        taskAlias = Console.ReadLine()!;
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskCreateAt);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskStart);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskForecastDate);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskDeadline);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskComplete);
                        Console.WriteLine("Enter  deliverables");
                        taskDeliverables = Console.ReadLine()!;
                        Console.WriteLine("Enter  required effort time");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out days);
                        requiredEffortTime = TimeSpan.FromDays(days);
                        Console.WriteLine("Enter remarks");
                        taskRemarks = Console.ReadLine()!;
                        Console.WriteLine("Enter engineer id");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out engineerInTaskId);
                        Console.WriteLine("Enter input for level");
                        inputEE = Console.ReadLine()!;
                        taskLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEE);
                        Console.WriteLine("Enter task in list id");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out taskInListId);
                        try
                        {
                            while (taskInListId != -1)
                            {
                                taskInList!.Add(new BO.TaskInList()
                                {
                                    Id = taskInListId,
                                    Description = s_bl.Task.Read(taskInListId)!.Description,
                                    Alias = s_bl.Task.Read(taskInListId)!.Alias,
                                    Status = Tools.CalculateStatus(null, null, null, null)
                                });
                                taskInListId = int.Parse(Console.ReadLine()!);
                            }
                            BO.Task newTask = new BO.Task()
                            {
                                Id = 0,
                                Description = taskDescription,
                                Alias = taskAlias,
                                CreateAt = DateTime.Now,
                                Start = null,
                                ForecastDate = null,
                                Deadline = null,
                                Complete = null,
                                Deliverables = taskDeliverables,
                                RequiredEffortTime = requiredEffortTime,
                                Remarks = taskRemarks,
                                Engineer = new EngineerInTask()
                                {
                                    Id = engineerInTaskId,
                                    Name = s_bl.Engineer.Read(engineerInTaskId)!.Name!
                                },
                                Level = taskLevel,
                                Status = Tools.CalculateStatus(null, null, null, null),
                                Milestone = null,
                                Dependencies = taskInList!
                            };
                            s_bl.Task.Create(newTask);
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                        break;
                    case 2://read task
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out id);
                        try
                        {
                            if (s_bl.Task!.Read(id) is null)
                                Console.WriteLine("no task found");
                            else
                                Console.WriteLine(s_bl.Task!.Read(id)!.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 3://read all task
                        try
                        {
                            s_bl.Task!.ReadAll()
                                      .ToList()
                                      .ForEach(task => Console.WriteLine(task.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    case 4://update task
                        int idTaskUpdate,
                             taskEngineerIdUpdate,
                              taskInListIdUpdate;
                        string? taskDescriptionUpdate,
                            taskAliasUpdate,
                            taskDeliverablesUpdate,
                            taskRemarksUpdate,
                            inputEEUpdate,
                            inputUpdate;
                        TimeSpan? requiredEffortTimeUpdate;
                        EngineerExperience? taskLevelUpdate;
                        try
                        {
                            List<BO.TaskInList?> taskInListUpdate = new List<BO.TaskInList?>();
                            Console.WriteLine("Enter id for reading");
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idTaskUpdate);
                            BO.Task updatedTask = s_bl.Task.Read(idTaskUpdate)!;
                            Console.WriteLine(updatedTask.ToString());
                            Console.WriteLine("Enter description to update");//if null to put the same details
                            taskDescriptionUpdate = Console.ReadLine();
                            if(taskDescriptionUpdate == null || taskDescriptionUpdate =="") {
                                taskDescriptionUpdate = updatedTask.Description;
                            }
                            Console.WriteLine("Enter alias to update");
                            taskAliasUpdate = Console.ReadLine();
                            if (taskAliasUpdate == null || taskAliasUpdate == "")
                            {
                                taskAliasUpdate = updatedTask.Alias;
                            }
                            Console.WriteLine("Enter required effort time to update");
                            inputUpdate = Console.ReadLine()!;
                            requiredEffortTimeUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.RequiredEffortTime : TimeSpan.FromDays(int.Parse(inputUpdate));

                            Console.WriteLine("Enter deliverables to update");
                            taskDeliverablesUpdate = Console.ReadLine() ?? updatedTask.Deliverables;
                            if (taskDeliverablesUpdate == null || taskDeliverablesUpdate == "")
                            {
                                taskDeliverablesUpdate = updatedTask.Deliverables;
                            }
                            Console.WriteLine("Enter remarks to update");
                            taskRemarksUpdate = Console.ReadLine() ?? updatedTask.Remarks;
                            if (taskRemarksUpdate == null || taskRemarksUpdate == "")
                            {
                                taskRemarksUpdate = updatedTask.Remarks;
                            }
                            Console.WriteLine("Enter input 1-5 to update the level");
                            inputEEUpdate = Console.ReadLine()!;
                            taskLevelUpdate = string.IsNullOrWhiteSpace(inputEEUpdate) ? updatedTask.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEEUpdate);
                            Console.WriteLine("entertask in list id");
                            inputUpdate = Console.ReadLine()!;

                            if(string.IsNullOrWhiteSpace(inputUpdate)) {
                                taskInListUpdate = updatedTask.Dependencies!;
                            }
                            else
                            {
                                taskInListIdUpdate = int.Parse(inputUpdate);
                                while (taskInListIdUpdate != -1)
                                {
                                    taskInListUpdate!.Add(new BO.TaskInList()
                                    {
                                        Id = taskInListIdUpdate,
                                        Description = s_bl.Task.Read(taskInListIdUpdate)!.Description,
                                        Alias = s_bl.Task.Read(taskInListIdUpdate)!.Alias,
                                        Status = Tools.CalculateStatus(updatedTask.Start, updatedTask.ForecastDate, updatedTask.Deadline, updatedTask.Complete)
                                    });
                                    int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out taskInListIdUpdate);
                                }
                            }
                            Console.WriteLine("Enter ID of engineer");
                            inputUpdate = Console.ReadLine()!;
                            taskEngineerIdUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.Engineer!.Id : int.Parse(inputUpdate);

                            BO.Task newTaskUpdate = new BO.Task()
                            {
                                Id = idTaskUpdate,
                                Description = taskDescriptionUpdate,
                                Alias = taskAliasUpdate,
                                CreateAt = updatedTask.CreateAt,
                                Start = updatedTask.Start,
                                ForecastDate = updatedTask.ForecastDate,
                                Deadline = updatedTask.Deadline,
                                Complete = updatedTask.Complete,
                                Deliverables = taskDeliverablesUpdate,
                                RequiredEffortTime = requiredEffortTimeUpdate,
                                Remarks = taskRemarksUpdate,
                                Engineer = new EngineerInTask()
                                {

                                    Id = taskEngineerIdUpdate,
                                    Name = s_bl.Engineer.Read(taskEngineerIdUpdate)!.Name!
                                },
                                Level = taskLevelUpdate,
                                Status = Tools.CalculateStatus(updatedTask.Start, updatedTask.ForecastDate, updatedTask.Deadline, updatedTask.Complete),
                                Milestone = updatedTask.Milestone,
                                Dependencies = taskInListUpdate!,
                            };
                            s_bl.Task.Update(newTaskUpdate);
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                        ; break;
                    case 5://delete task
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        try
                        {
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                            s_bl.Task!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine( ex);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        static void Main(string[] args)
        {
            DateTime start, end;
            Console.Write("Would you like to create Initial data? (Y/N)");//initialization
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            Console.WriteLine("TASK");
            TaskMenu();//calls to task menu
            Console.WriteLine("engineer");
            EngineerMenu();//calls to engineer menu
            Console.WriteLine("Enter start date for the project");//enter dates to start and end project
            DateTime.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a date"), out start);
            Console.WriteLine("Enter end date for the project");
            DateTime.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a date"), out end);
            Tools.EnterStartDateProject(start);
            Tools.EnterDeadLineDateProject(end);
            Console.WriteLine("Milestone");
            MilestoneMenu();//calls to milestone 
            Console.Write("would you like to change something more?  (Y/N)");
            string? ans2 = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans2 == "Y")
            {
                try
                {
                    int chooseEntity;
                    do
                    {
                        Console.WriteLine(" { EXIT, MILESTONE, ENGINEER, TASK }");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out chooseEntity);
                        switch (chooseEntity)
                        {
                            case 1:
                                MilestoneMenu();
                                break;
                            case 2:
                                EngineerMenu();
                                break;
                            case 3:
                                
                                TaskMenu();
                                break;
                            default: return;
                        }
                    } while (chooseEntity >= 0 && chooseEntity < 4);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
    }
              
    }
}