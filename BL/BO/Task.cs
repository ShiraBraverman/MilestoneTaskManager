namespace BO;

/// <summary>
/// מחלקה המייצגת משימה במערכת.
/// </summary>
public class Task
{
    // תכונה שמכילה את מזהה המשימה.
    public int Id { get; init; }

    // תכונה שמכילה את תיאור המשימה.
    public string Description { get; set; }

    // תכונה שמכילה את שם המשימה.
    public string Alias { get; set; }

    // תכונה שמכילה את תאריך יצירת המשימה.
    public DateTime CreateAt { get; set; }

    // תכונה שמכילה את תאריך התחלת המשימה.
    public DateTime? Start { get; set; } = null;

    // תכונה שמכילה את תאריך התחזקות כתובע משימה.
    public DateTime? ForecastDate { get; set; } = null;

    // תכונה שמכילה את תאריך סיום משימה.
    public DateTime? Deadline { get; set; } = null;

    // תכונה שמכילה את תאריך השלמת משימה.
    public DateTime? Complete { get; set; } = null;

    // תכונה שמכילה את סוגי המסמכים שיוצאים מהמשימה.
    public string? Deliverables { get; set; } = null;

    // תכונה שמכילה את זמן המאמץ הנדרש לביצוע המשימה.
    public TimeSpan? RequiredEffortTime { get; set; } = null;

    // תכונה שמכילה את הערות המשימה.
    public string? Remarks { get; set; } = null;

    // תכונה שמייצגת את המהנדס המשוייך למשימה.
    public EngineerInTask? Engineer { get; set; } = null;

    // תכונה שמייצגת את רמת הניסיון של המהנדס במשימה.
    public EngineerExperience? Level { get; set; } = null; 
    // תכונה שמייצגת את תפקיד המהנדס במשימה.
    public Roles? Role { get; set; } = null;

    // תכונה שמייצגת את הסטטוס של המשימה.
    public Status? Status { get; set; } = null;

    // תכונה שמייצגת את האבן דרך לה תלויות המשימה.
    public MilestoneInTask? Milestone { get; set; } = null;

    // רשימה שמייצגת את התלות של המשימה.
    public List<TaskInList>? Dependencies { get; set; } = null;

    // פונקציה שמחזירה מחרוזת המייצגת את המשימה.
    public override string ToString() => this.ToStringProperty();
}