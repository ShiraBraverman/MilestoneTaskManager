using System.Runtime.CompilerServices;
namespace BO;

// המחלקה Engineer מייצגת מהנדס בפרויקט.
public class Engineer
{
    // תכונה שמכילה את מזהה המהנדס.
    public int Id { get; init; }

    // תכונה שמכילה את שם המהנדס.
    public string? Name { get; set; }

    // תכונה שמכילה את כתובת האימייל של המהנדס.
    public string Email { get; set; }

    // תכונה שמכילה את רמת הניסיון של המהנדס.
    public EngineerExperience Level { get; set; }

    // תכונה שמכילה את עלות המהנדס לשעה.
    public double Cost { get; set; }

    // תכונה שמכילה את התפקיד של המהנדס.
    public Roles Role { get; set; }

    // תכונה שמכילה משימה שהוא אחראי עליה (אם יש).
    public TaskInEngineer? Task { get; set; } = null;

    // פונקציה שמחזירה מחרוזת המייצגת את האובייקט Engineer.
    public override string ToString() => this.ToStringProperty();
}