
using BlApi;
using System.Data;
using System.Collections.Generic;
using BO;
using DO;
using System.Xml.Linq;
using System.Reflection.Emit;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    // פונקציה Create: יצירת מהנדס חדש במסד הנתונים
    public int Create(BO.Engineer boEngineer)
    {
        // בדיקות תקינות
        Tools.ValidatePositiveId(boEngineer.Id, nameof(boEngineer.Id));
        Tools.ValidateNonEmptyString(boEngineer.Name, nameof(boEngineer.Name));
        Tools.ValidateEmail(boEngineer.Email, nameof(boEngineer.Email));
        Tools.ValidatePositiveNumber(boEngineer.Cost, nameof(boEngineer.Cost));

        // יצירת אובייקט DO.Engineer מתוך ה-BO שהתקבל
        DO.Engineer doEngineer = new DO.Engineer
        (boEngineer.Id,
        boEngineer.Name!,
        boEngineer.Email,
        (DO.EngineerExperience)boEngineer.Level,
        boEngineer.Cost,
        (DO.Roles)boEngineer.Role);

        try
        {
            // יצירת המהנדס במסד הנתונים והחזרת המזהה החדש שנוצר
            int newId = _dal.Engineer.Create(doEngineer);
            return newId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            // אם יש שגיאה שמציינת שהמהנדס כבר קיים, החזרת שגיאה עם המזהה שלו
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }

    // פונקציה Delete: מחיקת מהנדס ממסד הנתונים לפי מזהה
    public void Delete(int id)
    {
        try
        {
            // מחיקת המהנדס ממסד הנתונים לפי מזהה
            _dal.Engineer.Delete(id);
        }
        catch (Exception ex)
        {
            // אם יש שגיאה במחיקת המהנדס, החזרת שגיאה
            throw new BO.BlFailedToDelete($"Failed to delete engineer with ID={id}", ex);
        }
    }

    // פונקציה Read: קריאת מהנדס ממסד הנתונים לפי מזהה
    public BO.Engineer? Read(int id)
    {
        // קריאת המהנדס ממסד הנתונים לפי מזהה
        DO.Engineer? doEngineer = _dal.Engineer.Read(e => e.Id == id);

        // בדיקה האם המהנדס קיים
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        // יצירת אובייקט מהנדס מתוך ה-DO
        return CreateBOFromDO(doEngineer);
    }

    // פונקציה ReadAll: קריאת כל המהנדסים ממסד הנתונים
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        // יצירת פילטר, אם אין פילטר יותר נפשט ונקבל את כל המהנדסים
        Func<BO.Engineer, bool> filter1 = filter != null ? filter! : item => true;

        // החזרת רשימת המהנדסים ממסד הנתונים
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                let boEngineer = CreateBOFromDO(doEngineer)
                select boEngineer).Where(filter1).ToList();
    }

    // פונקציה Update: עדכון מהנדס במסד הנתונים
    public void Update(BO.Engineer boEngineer)
    {
        // בדיקות תקינות
        Tools.ValidatePositiveId(boEngineer.Id, nameof(boEngineer.Id));
        Tools.ValidateNonEmptyString(boEngineer.Name, nameof(boEngineer.Name));
        Tools.ValidateEmail(boEngineer.Email, nameof(boEngineer.Email));
        Tools.ValidatePositiveNumber(boEngineer.Cost, nameof(boEngineer.Cost));

        // יצירת אובייקט DO.Engineer מתוך ה-BO שהתקבל
        DO.Engineer newDoEngineer = new DO.Engineer
           (boEngineer.Id,
            boEngineer.Name!,
            boEngineer.Email,
            (DO.EngineerExperience)boEngineer.Level,
            boEngineer.Cost,
            (DO.Roles)boEngineer.Role);

        try
        {
            // עדכון המהנדס במסד הנתונים
            _dal.Engineer.Update(newDoEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            // אם יש שגיאה שמציינת שהמהנדס אינו קיים, החזרת שגיאה עם המזהה שלו
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} not exists", ex);
        }
    }

    // פונקציה פרטית: יצירת אובייקט מהנדס BO מתוך DO
    private BO.Engineer? CreateBOFromDO(DO.Engineer doEngineer)
    {
        // קריאה לכל המשימות ששייכות למהנדס
        var doTasks = _dal.Task.ReadAll(t => t.EngineerId == doEngineer.Id).FirstOrDefault();
        TaskInEngineer? taskInEngineer = null;

        // אם יש משימות, יצירת אובייקט משימה
        if (doTasks != null)
        {
            taskInEngineer = new BO.TaskInEngineer
            {
                Id = doTasks.Id,
                Description = doTasks.Description
            };
        }

        // יצירת אובייקט מהנדס BO מתוך DO
        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email!,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost ?? 0,
            Role = (BO.Roles)doEngineer.Role,
            Task = taskInEngineer
        };
    }
}