
using DalApi;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }  
    public IMilestone Milestone { get; }
    public ITask Task { get; }
}
