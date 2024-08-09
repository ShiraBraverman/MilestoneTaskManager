namespace BO;

public class TaskInList
{
    public int Id { get; init; }  // מזהה ייחודי למשימה ברשימה
    public string Description { get; set; }  // תיאור המשימה
    public string Alias { get; set; }  // שם המשימה (Alias)
    public BO.Status? Status { get; set; }  // סטטוס המשימה מתוך רשימת הסטטוסים ב-BO
    public override string ToString() => this.ToStringProperty();  // פונקצית ToString שמציגה את פרטי המשימה
}