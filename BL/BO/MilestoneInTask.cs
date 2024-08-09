namespace BO;

/// <summary>
/// מחלקה המייצגת אבן דרך בתוך משימה.
/// </summary>
public class MilestoneInTask
{
    // תכונה שמכילה את מזהה האבן דרך.
    public int Id { get; init; }

    // תכונה שמכילה את שם האבן דרך.
    public string Alias { get; set; }

    // פונקציה שמחזירה מחרוזת המייצגת את האובייקט MilestoneInTask.
    public override string ToString() => this.ToStringProperty();
}