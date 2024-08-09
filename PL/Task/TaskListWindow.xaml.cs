using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // רשימת המשימות המוצגת בחלון
        public ObservableCollection<BO.Task> TaskList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience EngExperience { get; set; } = BO.EngineerExperience.None;

        public BO.Status Status { get; set; } = BO.Status.None;

        public BO.Roles Role { get; set; } = BO.Roles.None;

        // פעולת התגובה ללחיצה על כפתור "הוספה"
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var taskWindow = new TaskWindow();
                taskWindow.Closed += (sender, e) => UpdateListAfterTaskWindowClosed();
                taskWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        // פעולת התגובה ללחיצה כפולה על אובייקט ברשימה לעדכון פרטיו
        private void GridUpdate_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
                var taskWindow = new TaskWindow(task!.Id);
                taskWindow.Closed += (sender, e) => UpdateListAfterTaskWindowClosed();
                taskWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        // עדכון רשימת המשימות לאחר סגירת חלון משימה
        private void UpdateListAfterTaskWindowClosed()
        {
            var temp = s_bl?.Task.ReadAll();
            TaskList = temp == null ? new() : new(temp!);
        }

        // פעולת התגובה לשינויים בבחירת הערכים בקומבובוקסים
        private void CbSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = (IEnumerable<BO.Task>?)null;
            if (EngExperience != BO.EngineerExperience.None && Role != BO.Roles.None && Status != BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Role == Role && item.Level == EngExperience && item.Status == Status)!;
            if (EngExperience != BO.EngineerExperience.None && Role != BO.Roles.None && Status == BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Role == Role && item.Level == EngExperience)!;
            if (EngExperience != BO.EngineerExperience.None && Role == BO.Roles.None && Status != BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Level == EngExperience && item.Status == Status)!;
            if (EngExperience != BO.EngineerExperience.None && Role == BO.Roles.None && Status == BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Level == EngExperience)!;
            if (EngExperience == BO.EngineerExperience.None && Role != BO.Roles.None && Status != BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Role == Role && item.Status == Status)!;
            if (EngExperience == BO.EngineerExperience.None && Role != BO.Roles.None && Status == BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Role == Role)!;
            if (EngExperience == BO.EngineerExperience.None && Role == BO.Roles.None && Status != BO.Status.None)
                temp = s_bl?.Task.ReadAll(item => item.Status == Status)!;
            if (EngExperience == BO.EngineerExperience.None && Role == BO.Roles.None && Status == BO.Status.None)
                temp = s_bl?.Task.ReadAll()!;
            TaskList = temp == null ? new() : new(temp!);
        }

        // פעולת בנייה
        public TaskListWindow()
        {
            InitializeComponent();
            var temp = s_bl?.Task.ReadAll();
            TaskList = temp == null ? new() : new(temp!);
        }
    }
}