using BO;
using System.Reflection;

public static class Tools
{
    // פונקציה שמחזירה מחרוזת המייצגת את התכולת האובייקט
    public static string ToStringProperty<T>(this T obj)
    {
        // קבלת מאפיינים של אובייקט מסוג T
        PropertyInfo[] properties = typeof(T).GetProperties();

        // בניית מחרוזת המייצגת את האובייקט
        string result = string.Join(", ", properties.Select(property =>
        {
            // קבלת הערך של המאפיין הנוכחי
            object? value = property.GetValue(obj);
            string valueString;

            // בדיקה האם הערך הוא null
            if (value == null)
            {
                valueString = "null";
            }
            // בדיקה האם הערך הוא קבוצה (IEnumerable)
            else if (value is IEnumerable<object> enumerableValue)
            {
                // אם זה כך, המרת כל איבר בקבוצה למחרוזת ושורפת יחודיות
                valueString = string.Join(", ", enumerableValue.Select(item => item.ToString()));
            }
            // אם אין קבוצה או null, המרת הערך למחרוזת
            else
            {
                valueString = value.ToString();
            }
            // בניית המחרוזת שמייצגת את המאפיין והערך שלו
            return $"{property.Name}: {valueString}";
        }));
        // החזרת התוצאה
        return result;
    }

    // פונקציה שבודקת אם המספר חיובי
    public static void ValidatePositiveId(int? id, string paramName)
    {
        if (id <= 0)
            throw new BO.BlInvalidDataException($"Invalid {paramName} ID. Must be a positive number.");
    }

    // פונקציה שבודקת אם המחרוזת לא ריקה או מכילה רק רווחים
    public static void ValidateNonEmptyString(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BO.BlInvalidDataException($"{paramName} cannot be empty.");
    }

    // פונקציה שבודקת אם המספר חיובי
    public static void ValidatePositiveNumber(double? number, string paramName)
    {
        if (number < 0)
            throw new BO.BlInvalidDataException($"Invalid {paramName}. Must be a positive number.");
    }

    // פונקציה שבודקת תקינות כתובת האימייל
    public static void ValidateEmail(string? email, string paramName)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                throw new BO.BlInvalidDataException($"Invalid email address for {paramName}.");
        }
        catch
        {
            throw new BO.BlInvalidDataException($"Invalid email address for {paramName}.");
        }
    }

    // פונקציה שמחשבת ומחזירה את הסטטוס של משימה לפי המצב שלה
    public static Status CalculateStatus(DateTime? start, DateTime? forecastDate, DateTime? deadline, DateTime? complete)
    {
        if (start != null && complete == null) // אם המשימה באמצע להעשות 
            return Status.OnTrack;

        if (complete != null) // אם המשימה הושלמה 
            return Status.Completed;

        if (complete == null && DateTime.Now > forecastDate) // אם המשימה עוד לא נגמרה וכבר עבר התאריך המתכונן לסיום
            return Status.InJeopardy;

        if (forecastDate == null && deadline == null) // אם המשימה עוד לא בלוז
            return Status.Unscheduled;

        if (forecastDate != null && deadline != null) // אם המשימה כבר בלוז
            return Status.Scheduled;

        return Status.Unscheduled;
    }

    public static List<BO.TaskInList>? CalculateList(int taskId)
    {
        // קבלת אובייקט IDal מהמפעיל Factory
        DalApi.IDal _dal = Factory.Get;

        // יצירת רשימה לאחסון המשימות
        List<BO.TaskInList>? tasksList = new List<BO.TaskInList>();
        // קריאת כל התלות של משימות שהן תלויות במשימה עם הזהות taskId
        _dal.Dependency.ReadAll(d => d.DependentTask == taskId)
                           // קבלת רשימת משימות שהן תלויות במשימה עם הזהות taskId
                           .Select(d => _dal.Task.Read(d1 => d1.Id == d.DependsOnTask))
                           .ToList()
                           .ForEach(task =>
                           {
                               // הוספת TaskInList לרשימה tasksList
                               tasksList.Add(new BO.TaskInList()
                               {
                                   Id = task.Id,
                                   Alias = task.Alias,
                                   Description = task.Description,
                                   // חישוב והצבת סטטוס בהתאם לפונקציה CalculateStatus
                                   Status = (BO.Status)Tools.CalculateStatus(task.Start, task.ForecastDate, task.Deadline, task.Complete)
                               });
                           });
        // החזרת הרשימה tasksList
        return tasksList;
    }

    // פונקציה שמציינת את תאריך ההתחלה של הפרויקט
    public static void EnterStartDateProject(DateTime startDate)
    {
        DalApi.IDal _dal = Factory.Get;
        _dal.startProject = startDate;
    }

    // פונקציה שמציינת את תאריך הסיום הכללי של הפרויקט
    public static void EnterDeadLineDateProject(DateTime deadlineProject)
    {
        DalApi.IDal _dal = Factory.Get;
        _dal.deadlineProject = deadlineProject;
    }
}