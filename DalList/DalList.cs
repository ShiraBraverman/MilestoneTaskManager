using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace Dal;
//sealed internal class DalList : IDal
//{
//    public static IDal Instance { get; } = new DalList();
//    private DalList() { }
//    public IDependency Dependency => new DependencyImplementation();

//    public IEngineer Engineer => new EngineerImplementation();

//    public ITask Task => new TaskImplementation();
//}

sealed internal class DalList : IDal
{
    private static readonly Lazy<DalList> lazyInstance = new Lazy<DalList>(() => new DalList(), true);

    private static readonly IDependency dependencyInstance = new DependencyImplementation();
    private static readonly IEngineer engineerInstance = new EngineerImplementation();
    private static readonly ITask taskInstance = new TaskImplementation();
    

    static DalList()
    {
        
    }

    public IDependency Dependency => dependencyInstance;
    public IEngineer Engineer => engineerInstance;
    public ITask Task => taskInstance;
    public DateTime? startProject { get => DataSource.Config.startProject; set => DataSource.Config.startProject = value; }
    public DateTime? deadlineProject { get => DataSource.Config.deadlineProject; set => DataSource.Config.deadlineProject = value; }
    public static IDal Instance => lazyInstance.Value;

    private DalList() { }

    public void Reset()
    {
        Task.Reset();
        Engineer.Reset();
        Dependency.Reset();
    }
}