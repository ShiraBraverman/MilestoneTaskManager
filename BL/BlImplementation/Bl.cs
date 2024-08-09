
namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone =>  new MilestoneImplementation();

    public ITask Task => new TaskImplementation();
}
