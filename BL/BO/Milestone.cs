namespace BO;

/// <summary>
/// מחלקה המייצגת מסלול מעקב אחרי אבן דרך בפרויקט.
/// </summary>
public class Milestone
{
    // תכונה שמכילה את מזהה האבן דרך.
    public int Id { get; init; }

    // תכונה שמכילה את התיאור של האבן דרך.
    public string Description { get; set; }

    // תכונה שמכילה את שם האבן דרך.
    public string Alias { get; set; }

    // תכונה שמכילה את תאריך היצירה של האבן דרך.
    public DateTime CreateAt { get; set; }

    // תכונה שמכילה את סטטוס האבן דרך.
    public Status? Status { get; set; } = null;

    // תכונה שמכילה את תאריך התחזקות האבן דרך (תחזקות פנימית של הצפי).
    public DateTime? ForecastDate { get; set; } = null;

    // תכונה שמכילה את המועד הסופי להשלמת האבן דרך.
    public DateTime? Deadline { get; set; } = null;

    // תכונה שמכילה את תאריך השלמת האבן דרך.
    public DateTime? Complete { get; set; } = null;

    // תכונה שמכילה את אחוז ההשלמה של האבן דרך.
    public double? CompletionPercentage { get; set; } = null;

    // תכונה שמכילה את ההערות הנוספות על האבן דרך.
    public string? Remarks { get; set; } = null;

    // תכונה שמכילה רשימת התלות של האבן דרך.
    public List<TaskInList?>? Dependencies { get; set; } = null;

    // פונקציה שמחזירה מחרוזת המייצגת את האובייקט Milestone.
    public override string ToString() => this.ToStringProperty();
}