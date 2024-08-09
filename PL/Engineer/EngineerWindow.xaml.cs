using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer;
/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    // משתנה סטטי לממשק הלוגיקה של הבין-לקוח (BL)
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // משתנה המציין אם מדובר בהוספת מהנדס חדש או עדכון של מהנדס קיים
    bool isAdding = false;

    // מאפיינים של המהנדס
    public BO.Engineer Engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

    // רשימת המשימות של המהנדס
    public ObservableCollection<BO.TaskInList> EngineerTasks
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(EngineerTasksProperty); }
        set { SetValue(EngineerTasksProperty, value); }
    }
    public static readonly DependencyProperty EngineerTasksProperty =
        DependencyProperty.Register("EngineerTasks", typeof(ObservableCollection<BO.TaskInList>), typeof(EngineerWindow), new PropertyMetadata(null));

    // ניסיון המהנדס
    public BO.EngineerExperience EngExperience { get; set; }

    // תפקיד המהנדס
    public BO.Roles Role { get; set; }

    // פונקציה המתבצעת כאשר משתנה הבחירה בניסיון המהנדס
    private void ComboBoxEngExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Engineer != null)
            Engineer.Level = EngExperience == BO.EngineerExperience.None ? Engineer.Level : EngExperience;
    }

    // פונקציה המתבצעת כאשר משתנה הבחירה בתפקיד המהנדס
    private void ComboBoxRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Engineer != null)
            Engineer.Role = Role == BO.Roles.None ? Engineer.Role : Role;
    }

    // פונקציה המתבצעת כאשר לוחצים על כפתור המשימות
    private void BtnTaskWindow_List(object sender, RoutedEventArgs e)
    {
        if (Engineer.Task != null)
        {
            try
            {
                // פתיחת חלון המשימות הקשורות למהנדס זה
                var taskWindow = new TaskWindow(Engineer.Task!.Id);
                taskWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }
    }

    // פונקציה המתבצעת כאשר לוחצים על כפתור הוספת או עדכון מהנדס
    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (isAdding)
            {
                // הוספת מהנדס חדש
                s_bl.Engineer.Create(Engineer);
                MessageBox.Show("addition successful", "Confirmation", MessageBoxButton.OK);
            }
            else
            {
                // עדכון פרטי מהנדס קיים
                s_bl.Engineer.Update(Engineer);
                MessageBox.Show("updation successful", "Confirmation", MessageBoxButton.OK);
            }
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
        }
    }

    // בנאי החלון שמקבל מזהה מהנדס (או 0 אם מדובר בהוספת מהנדס חדש)
    public EngineerWindow(int id = 0)
    {
        try
        {
            if (id != 0)
                Engineer = s_bl!.Engineer.Read(id)!;
            else
            {
                // אם מדובר בהוספת מהנדס חדש
                Engineer = new BO.Engineer();
                isAdding = true;
            }
            EngExperience = Engineer.Level;
            Role = Engineer.Role;

            // קביעת רשימת המשימות של המהנדס
            var temp = s_bl?.Task.ReadAll(t => t.Engineer == null ? false : t.Engineer.Id == Engineer.Id).Select(t => new BO.TaskInList()
            {
                Id = t.Id,
                Alias = t.Alias,
                Description = t.Description,
                Status = t.Status
            }).ToList();
            EngineerTasks = temp != null ? new ObservableCollection<BO.TaskInList>(temp) : new ObservableCollection<BO.TaskInList>();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
        }
        InitializeComponent();
    }

    // פונקציה המתבצעת כאשר לוחצים פעמיים על אחת המשימות ברשימה
    private void TasksListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem != null)
        {
            try
            {
                if (Engineer.Task != null)
                {
                    BO.Task? task = s_bl!.Task.Read(Engineer.Task.Id);
                    if (task != null && task.Status != BO.Status.Completed)
                        MessageBox.Show("You can't start this task before finish the last one.", "Confirmation", MessageBoxButton.OK);
                    else
                        MessageBox.Show("After you will close the task window you will be able to start it.", "Confirmation", MessageBoxButton.OK);
                }
                else
                    MessageBox.Show("After you will close the task window you will be able to start it.", "Confirmation", MessageBoxButton.OK);

                // קביעת מזהה המשימה שנבחרה
                int selectedTaskId = ((BO.TaskInList)listBox.SelectedItem).Id;

                // פתיחת חלון המשימה הנבחרת
                var taskWindow = new TaskWindow(selectedTaskId);
                taskWindow.Closed += (sender, e) => ChangeTask(selectedTaskId);
                taskWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
            listBox.SelectedItem = null;
        }
    }

    // פונקציה שמתבצעת לאחר סגירת חלון המשימה ומתקבלת מזהה המשימה ששונתה בחלון
    private void ChangeTask(int selectedTaskId)
    {
        if (Engineer.Task != null)
        {
            BO.Task? currentTask = s_bl!.Task.Read(Engineer.Task.Id);
            if (currentTask != null && currentTask.Status != BO.Status.Completed)
            {
                // בדיקה האם המשימה הנוכחית של המהנדס הושלמה והאם ניתן להתחיל במשימה החדשה
                MessageBoxResult result = MessageBox.Show("Do you want to start this Task?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // קביעת התחלת המשימה החדשה
                    BO.Task? task = s_bl!.Task.Read(selectedTaskId);
                    task!.Start = DateTime.Now;
                    s_bl!.Task.Update(task!);
                }
            }
        }
    }
}