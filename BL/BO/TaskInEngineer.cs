namespace BO;

public class TaskInEngineer
{
    public int Id { get; init; }  // זה מזהה ייחודי למשימה בתוך המהנדס
    public string Description { get; set; }  // שם המשימה (Alias)
    public override string ToString() => this.ToStringProperty();
}