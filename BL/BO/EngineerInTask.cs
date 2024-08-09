namespace BO;

// המחלקה EngineerInTask מייצגת מהנדס שמשוייך למשימה מסוימת.
public class EngineerInTask
{
    // תכונה שמכילה את מזהה המהנדס.
    public int Id { get; init; }

    // תכונה שמכילה את שם המהנדס.
    public string? Name { get; set; }

    // פונקציה שמחזירה מחרוזת המייצגת את האובייקט EngineerInTask.
    public override string ToString() => this.ToStringProperty();
}