namespace DO;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Description"></param>
/// <param name="Alias"></param>
/// <param name="Milestone"></param>
/// <param name="CreateAt"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="Level"></param>
/// <param name="Role"></param>
/// <param name="Start"></param>
/// <param name="ForecastDate"></param>
/// <param name="Deadline"></param>
/// <param name="Complete"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
/// <param name="EngineerId"></param>
public record Task
    (
    int Id,
    string Description,
    string Alias,
    bool Milestone,
    DateTime CreateAt,
    TimeSpan? RequiredEffortTime=null,
    EngineerExperience? Level = null,
    Roles? Role = null,
    DateTime? Start = null,
    //DateTime? ScheduledDate = null,
    //DateTime? BaselineStartDate = null,
    DateTime? ForecastDate = null,
    DateTime? Deadline = null,
    DateTime? Complete = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null 
 )
{
    public Task() : this(0, "", "", false, new DateTime(2024,1,1) ,TimeSpan.Zero) { }
}
 
