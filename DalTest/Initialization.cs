namespace DalTest;
using DalApi;
using DO;

using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

public static class Initialization
{
    //private static IDependency? s_dalDependency; //stage 1
    //private static IEngineer? s_dalEngineer; //stage 1
    //private static ITask? s_dalTask; //stage 1
    private static IDal? s_dal; //stage 2
                                //private static readonly Random s_rand = new();

    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dalDependency = _s_dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalEngineer = _s_dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalTask = _s_dalTask ?? throw new NullReferenceException("DAL can not be null!");
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = Factory.Get; //stage 4

        s_dal.Reset();
        createEngineers();
        createTasks();
        createDependencies();
    }

    private static void createEngineers()
    {
        s_dal!.Engineer.Create(new Engineer(248728764, "Rony Gilbert", "ronygil64@exam.com", EngineerExperience.Expert, 150.5, Roles.TeamLeader));
        s_dal.Engineer.Create(new Engineer(982485477, "Meir Fuks", "meirfuks@exam.com", EngineerExperience.AdvancedBeginner, 130.5, Roles.GraphicArtist));
        s_dal.Engineer.Create(new Engineer(165324683, "Edi Green", "edi24683@exam.com", EngineerExperience.Advanced, 140.5, Roles.Programmer));
        s_dal.Engineer.Create(new Engineer(934759393, "Shimmy Lipsin", "sl934759393@exam.com", EngineerExperience.Beginner, 100.5, Roles.Programmer));
        s_dal.Engineer.Create(new Engineer(113634844, "Dudi Dlin", "dudidlin844@exam.com", EngineerExperience.Beginner, 100.5, Roles.Programmer));
    }

    private static void createTasks()
    {
        s_dal!.Task.Create(new Task(0, "Decide what is the next project", "Alias1", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(10), EngineerExperience.Expert, Roles.TeamLeader, null, null, null, null, null, null, 248728764));
        s_dal.Task.Create(new Task(0, "Check the requirements", "Alias2", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(20), EngineerExperience.AdvancedBeginner, Roles.TeamLeader, null, null, null, null, null, null, 982485477));
        s_dal.Task.Create(new Task(0, "Choose the most convenient way", "Alias3", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(3), EngineerExperience.AdvancedBeginner, Roles.TeamLeader, null, null, null, null, null, null, 982485477));
        s_dal.Task.Create(new Task(0, "Decide on the location of the feature", "Alias4", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(5), EngineerExperience.AdvancedBeginner, Roles.TeamLeader, null, null, null, null, null, null, 982485477));
        s_dal.Task.Create(new Task(0, "Decide on the shape of the feature", "Alias5", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(2), EngineerExperience.AdvancedBeginner, Roles.TeamLeader, null, null, null, null, null, null, 982485477));
        s_dal.Task.Create(new Task(0, "Work on the design", "Alias6", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(100), EngineerExperience.AdvancedBeginner, Roles.GraphicArtist, null, null, null, null, null, null, 982485477));
        s_dal.Task.Create(new Task(0, "stage 0 in the programming", "P0", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 1 in the programming", "P1", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 2 in the programming", "P2", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 3 in the programming", "P3", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 4 in the programming", "P4", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 5 in the programming", "P5", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 6 in the programming", "P6", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "stage 7 in the programming", "P6", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(30), EngineerExperience.Advanced, Roles.Programmer, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "Run the code", "Alias15", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(20), EngineerExperience.Beginner, Roles.Programmer, null, null, null, null, null, null, 934759393));
        s_dal.Task.Create(new Task(0, "Find errors in the code ", "Alias16", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(20), EngineerExperience.Beginner, Roles.Programmer, null, null, null, null, null, null, 934759393));
        s_dal.Task.Create(new Task(0, "Get permission from the programmer and pass the code on", "Alias17", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(6), EngineerExperience.Beginner, Roles.Programmer, null, null, null, null, null, null, 934759393));
        s_dal.Task.Create(new Task(0, "Bring confirmation to the software tester that the code is correct", "Alias18", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(5), EngineerExperience.Beginner, Roles.TeamLeader, null, null, null, null, null, null, 165324683));
        s_dal.Task.Create(new Task(0, "Send the feature to the advertising team", "Alias19", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(3), EngineerExperience.Beginner, Roles.TeamLeader, null, null, null, null, null, null, 934759393));
        s_dal.Task.Create(new Task(0, "Update all platforms", "Alias20", false, new DateTime(2024, 1, 1), TimeSpan.FromDays(2), EngineerExperience.Beginner, Roles.TeamLeader, null, null, null, null, null, null, 113634844));
    }

    private static void createDependencies()
    {
        s_dal!.Dependency.Create(new Dependency(0, 2, 1));
        s_dal.Dependency.Create(new Dependency(0, 3, 2));
        s_dal.Dependency.Create(new Dependency(0, 4, 3));
        s_dal.Dependency.Create(new Dependency(0, 5, 4));
        s_dal.Dependency.Create(new Dependency(0, 7, 5));
        s_dal.Dependency.Create(new Dependency(0, 8, 7));
        s_dal.Dependency.Create(new Dependency(0, 9, 8));
        s_dal.Dependency.Create(new Dependency(0, 10, 9));
        s_dal.Dependency.Create(new Dependency(0, 12, 11));
        s_dal.Dependency.Create(new Dependency(0, 13, 10));
        s_dal.Dependency.Create(new Dependency(0, 14, 13));
        s_dal.Dependency.Create(new Dependency(0, 15, 14));
        s_dal.Dependency.Create(new Dependency(0, 15, 20));
        s_dal.Dependency.Create(new Dependency(0, 16, 14));
        s_dal.Dependency.Create(new Dependency(0, 20, 18));
        s_dal.Dependency.Create(new Dependency(0, 20, 19));
        s_dal.Dependency.Create(new Dependency(0, 18, 7));
        s_dal.Dependency.Create(new Dependency(0, 19, 17));
    }
}