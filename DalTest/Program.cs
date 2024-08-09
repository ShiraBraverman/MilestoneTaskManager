using Dal;
using DO;
using DalApi;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System;


namespace DalTest
{
    internal class Program
    {
        //static readonly IDal s_dal = new Dal.DalList(); //stage 2
        //static readonly IDal s_dal = new Dal.DalXml(); //stage 3
        static readonly IDal s_dal = Factory.Get; //stage 4
        //private static IDependency? s_dalDependency = new DependnecyImplementation(); //stage 1
        //private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        //private static ITask? s_dalTask = new TaskImplementation(); //stage 1

        enum MainMenu { INITIALIZATION, EXIT, DEPENDENCY, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }
        Random random = new Random();

        private static void EngineerMenu()
        {

            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new Exception("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter id, name, email, cost and a number to choose experience");
                        int idEngineer;
                        string nameEngineer, emailEngineer, input;
                        EngineerExperience levelEngineer;
                        bool isActive;
                        double costEngineer;
                        idEngineer = int.Parse(Console.ReadLine());
                        nameEngineer = (Console.ReadLine());
                        emailEngineer = Console.ReadLine();
                        input = Console.ReadLine();
                        levelEngineer = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), input);
                        costEngineer = double.Parse(Console.ReadLine());
                        s_dal.Engineer.Create(new Engineer(idEngineer, nameEngineer, emailEngineer, levelEngineer, costEngineer));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Engineer!.Read(e => e.Id == id) is null)
                            Console.WriteLine("no engineer found");
                        else
                            Console.WriteLine(s_dal.Engineer!.Read(e => e.Id == id).ToString());
                        break;
                    case 3:
                        s_dal.Engineer!.ReadAll()
         .ToList()
         .ForEach(engineer => Console.WriteLine(engineer.ToString()));
                        break;
                    case 4:
                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate, inputUpdate;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine());
                        Engineer updatedEngineer = s_dal.Engineer.Read(e => e.Id == idEngineerUpdate);
                        Console.WriteLine(updatedEngineer.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        nameEngineerUpdate = Console.ReadLine() ?? updatedEngineer?.Name;
                        emailEngineerUpdate = Console.ReadLine() ?? updatedEngineer?.Email;
                        inputUpdate = Console.ReadLine();
                        levelEngineerUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedEngineer.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdate);
                        costEngineerUpdate = double.Parse(Console.ReadLine());


                        s_dal.Engineer.Update(new Engineer(idEngineerUpdate, nameEngineerUpdate, emailEngineerUpdate, levelEngineerUpdate, costEngineerUpdate));
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Engineer!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        private static void DependencyMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                chooseSubMenu = int.Parse(Console.ReadLine());

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter details for all the characteristics");
                        int dependentTask, dependsOnTask;
                        dependentTask = int.Parse(Console.ReadLine());
                        dependsOnTask = int.Parse(Console.ReadLine());
                        s_dal.Dependency.Create(new Dependency(0, dependentTask, dependsOnTask));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Dependency!.Read(d => d.Id == id) is null)
                            Console.WriteLine("no dependency found");
                        else
                            Console.WriteLine(s_dal.Dependency!.Read(d => d.Id == id).ToString());
                        break;
                    case 3:
                        s_dal.Dependency!.ReadAll()
       .ToList()
       .ForEach(dependency => Console.WriteLine(dependency.ToString()));
                        break;
                    case 4:
                        int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;
                        Console.WriteLine("Enter id for reading");
                        idUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Dependency!.Read(d => d.Id == idUpdate).ToString());
                        Console.WriteLine("Enter details to update");
                        dependentTaskUpdate = int.Parse(Console.ReadLine());
                        dependsOnTaskUpdate = int.Parse(Console.ReadLine());
                        s_dal.Dependency!.Update(new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate));
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Dependency!.Delete(idDelete);
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
                chooseSubMenu = int.Parse(Console.ReadLine());

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter  description, alias,deriverables, remarks,milestone, dates and task's level");
                        int taskEngineerId;
                        string taskDescription, taskAlias, taskDeliverables, taskRemarks, input;
                        bool taskMilestone;
                        TimeSpan requiredEffortTime;
                        DateTime taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete;
                        EngineerExperience taskLevel;
                        taskDescription = Console.ReadLine();
                        taskAlias = Console.ReadLine();
                        taskMilestone = bool.Parse(Console.ReadLine());
                        requiredEffortTime = TimeSpan.Zero;
                        input = Console.ReadLine();
                        taskLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), input);
                        taskCreateAt = DateTime.Parse(Console.ReadLine());
                        taskStart = DateTime.Parse(Console.ReadLine());
                        taskForecastDate = DateTime.Parse(Console.ReadLine());
                        taskDeadline = DateTime.Parse(Console.ReadLine());
                        taskComplete = DateTime.Parse(Console.ReadLine());
                        taskDeliverables = Console.ReadLine();
                        taskRemarks = Console.ReadLine();
                        taskEngineerId = int.Parse(Console.ReadLine());

                        s_dal.Task.Create(new DO.Task(0, taskDescription, taskAlias, taskMilestone, taskCreateAt, requiredEffortTime, taskLevel,null, taskStart, taskForecastDate, taskDeadline, taskComplete, taskDeliverables, taskRemarks, taskEngineerId));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Task!.Read(t => t.Id == id) is null)
                            Console.WriteLine("no task found");
                        else
                            Console.WriteLine(s_dal.Task!.Read(t => t.Id == id).ToString());
                        break;
                    case 3:
                        s_dal.Task!.ReadAll()
        .ToList()
        .ForEach(task => Console.WriteLine(task.ToString()));
                        break;
                    case 4:
                        int idTaskUpdate, taskEngineerIdUpdate;
                        string taskDescriptionUpdate, taskAliasUpdate, taskDeliverablesUpdate, taskRemarksUpdate, inputUpdate;
                        bool taskMilestoneUpdate;
                        DateTime taskCreateAtUpdate, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate;
                        TimeSpan requiredEffortTimeUpdate;
                        EngineerExperience? taskLevelUpdate;
                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine());
                        DO.Task updatedTask = s_dal.Task.Read(t => t.Id == idTaskUpdate);
                        Console.WriteLine(updatedTask.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskDescriptionUpdate = Console.ReadLine();
                        taskAliasUpdate = Console.ReadLine();
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine());
                        requiredEffortTimeUpdate = TimeSpan.Zero;
                        inputUpdate = Console.ReadLine();
                        taskLevelUpdate = string.IsNullOrWhiteSpace(inputUpdate) ? updatedTask.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdate);
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine());
                        taskStartUpdate = DateTime.Parse(Console.ReadLine());
                        taskForecastDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine());
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeliverablesUpdate = Console.ReadLine();
                        taskRemarksUpdate = Console.ReadLine();
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine());

                        s_dal.Task.Update(new DO.Task(idTaskUpdate, taskDescriptionUpdate, taskAliasUpdate, taskMilestoneUpdate, taskCreateAtUpdate, requiredEffortTimeUpdate, taskLevelUpdate,null, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate, taskDeliverablesUpdate, taskRemarksUpdate, taskEngineerIdUpdate)); break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Task!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        static void Main(string[] args)
        {
            try
            {
                int chooseEntity;
                do
                {
                    Console.WriteLine("enum MainMenu { INITIALIZATION, EXIT, DEPENDENCY, ENGINEER, TASK }");
                    chooseEntity = int.Parse(Console.ReadLine());

                    switch (chooseEntity)
                    {
                        case 0:
                            //Initialization.Do(s_dalStudent, s_dalCourse, s_dalLink); //stage 1
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y") //stage 3
                                            //Initialization.Do(s_dal); //stage 2
                                Initialization.Do(); //stage 4
                            break;
                        case 2:
                            DependencyMenu();
                            break;
                        case 3:
                            EngineerMenu();
                            break;
                        case 4:
                            TaskMenu();
                            break;
                    }
                } while (chooseEntity >= 0 && chooseEntity < 4);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //string dependenciesFilePath = @"../xml/dependencies.xml";
            //CreateDependencies(dependenciesFilePath, 40); 


            //List<XElement> tasks = GenerateRandomTasks(20);

            //XElement arrayOfTask = new XElement("ArrayOfTask", tasks);
            //XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), arrayOfTask);

            //xmlDocument.Save(@"../xml/tasks.xml");
        }

        //static Random rand = new Random();

        //static void CreateDependencies(string filePath, int count)
        //{
        //    XElement dependenciesElement = new XElement("ArrayOfDependency");

        //    for (int i = 0; i < count; i++)
        //    {
        //        int taskId = i + 1;
        //        int dependentTaskId = GetRandomTaskId(taskId);
        //        XElement dependencyElement = new XElement("Dependency",
        //            new XElement("Id", i + 1),
        //            new XElement("DependentTaskId", dependentTaskId),
        //            new XElement("DependsOnTaskId", taskId)
        //        );

        //        dependenciesElement.Add(dependencyElement);
        //    }

        //    dependenciesElement.Save(filePath);
        //}

        //static int GetRandomTaskId(int currentTaskId)
        //{
        //    int randomOffset = rand.Next(20); // 20 משימות בסך הכול
        //    return (currentTaskId + randomOffset) % 20;
        //}

        //static List<XElement> GenerateRandomTasks(int count)
        //{
        //    List<XElement> tasks = new List<XElement>();
        //    Random random = new Random();

        //    for (int i = 0; i < count; i++)
        //    {
        //        XElement task = new XElement("Task",
        //            new XElement("Id", 0),
        //            new XElement("Description", $"Task {i}"),
        //            new XElement("Alias", $"Alias{i}"),
        //            new XElement("Milestone", false),
        //            new XElement("RequiredEffortTime", "PT0S"),
        //            new XElement("CreateAt", DateTime.Now.AddHours(random.Next(1, 48))),
        //            new XElement("Start", DateTime.Now.AddHours(random.Next(48, 96))),
        //            new XElement("ForecastDate", DateTime.Now.AddHours(random.Next(96, 144))),
        //            new XElement("Deadline", DateTime.Now.AddHours(random.Next(144, 192))),
        //            new XElement("Complete", DateTime.Now.AddHours(random.Next(192, 240))),
        //            new XElement("Deliverables", $"Deliverable{i}"),
        //            new XElement("Remarks", $"Remark{i}"),
        //            new XElement("Level", (EngineerExperience)random.Next(0, 5)),
        //            new XElement("isActive", true)
        //        );

        //        tasks.Add(task);
        //    }

        //    return tasks;
        //}
    }
}