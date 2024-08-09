using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // מאפיינים של המשימה
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        // מהנדס שמבצע את המשימה
        public BO.EngineerInTask? Engineer
        {
            get { return (BO.EngineerInTask)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata(null));

        // רשימת המשימות התלויות
        public ObservableCollection<BO.TaskInList> TaskDependencies
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskDependenciesProperty); }
            set { SetValue(TaskDependenciesProperty, value); }
        }

        public static readonly DependencyProperty TaskDependenciesProperty =
            DependencyProperty.Register("TaskDependencies", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

        // רשימת המהנדסים לבחירה
        public ObservableCollection<BO.Engineer> EngineersList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineersListProperty); }
            set { SetValue(EngineersListProperty, value); }
        }

        public static readonly DependencyProperty EngineersListProperty =
            DependencyProperty.Register("EngineersList", typeof(ObservableCollection<BO.Engineer>), typeof(TaskWindow), new PropertyMetadata(null));

        // רשימת כל המשימות לבחירת משימה תלות
        public ObservableCollection<BO.TaskInList> TasksList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register("TasksList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.EngineerExperience EngExperience { get; set; }
        public int EngineerId { get; set; } = 0;
        public int DepTask { get; set; } = 0;
        public BO.Roles Role { get; set; }

        // פעולות האירועים בקונטרולים
        private void ComboBoxEngExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task.Level = EngExperience;
            if (Task != null && Task.Level != null)
                LevelChanged(EngExperience);
        }

        private void ComboBoxRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task.Role = Role;
            if (Task != null && Task.Role != null)
                RoleChanged(Role);
        }

        private void ComboBoxEngineer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (EngineerId != 0)
                {
                    BO.Engineer eng = s_bl.Engineer.Read(EngineerId)!;
                    Engineer = new BO.EngineerInTask() { Id = eng.Id, Name = eng.Name };
                    Task.Engineer = Engineer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        private void ComboBoxDepTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DepTask != 0)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to add the selected item?", "Confirmation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        BO.Task dep = s_bl.Task.Read(DepTask)!;
                        if (Task.Dependencies == null)
                            Task.Dependencies = new List<TaskInList>();

                        Task.Dependencies.Add(new BO.TaskInList()
                        {
                            Id = dep.Id,
                            Alias = dep.Alias,
                            Description = dep.Description,
                            Status = dep.Status
                        });
                        TaskDependencies = new ObservableCollection<BO.TaskInList>(Task.Dependencies);
                    }
                    DepTask = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        private void DependenciesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete the selected item?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Task.Dependencies!.Remove((BO.TaskInList)listBox.SelectedItem);
                    TaskDependencies = new ObservableCollection<BO.TaskInList>(Task.Dependencies);
                }

                listBox.SelectedItem = null;
            }
        }

        private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Task.Id == 0)
                {
                    s_bl.Task.Create(Task);
                    MessageBox.Show("addition successful", "Confirmation", MessageBoxButton.OK);
                }
                else
                {
                    s_bl.Task.Update(Task);
                    MessageBox.Show("updation successful", "Confirmation", MessageBoxButton.OK);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        // בנאי החלון
        public TaskWindow(int id = 0)
        {
            try
            {
                if (id != 0)
                    Task = s_bl!.Task.Read(id)!;
                else
                    Task = new BO.Task() { CreateAt = DateTime.Now };
                Role = Task.Role == null ? Roles.None : Task.Role.Value;
                EngExperience = Task.Level == null ? EngineerExperience.None : Task.Level.Value;
                TaskDependencies = Task.Dependencies != null ? new ObservableCollection<BO.TaskInList>(Task.Dependencies) : new ObservableCollection<BO.TaskInList>();
                Engineer = Task.Engineer != null ? Task.Engineer : null;
                var temp = s_bl?.Task.ReadAll().Select(t => new BO.TaskInList()
                {
                    Id = t.Id,
                    Alias = t.Alias,
                    Description = t.Description,
                    Status = t.Status
                }).ToList();
                TasksList = temp != null ? new ObservableCollection<BO.TaskInList>(temp) : new ObservableCollection<BO.TaskInList>();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
            InitializeComponent();
        }

        // מתודות פרטיות לטיפול בשינויי ערכים בתכונות
        private void RoleChanged(Roles value)
        {
            if (value == Roles.None)
                Task.Role = null;
            else
                Task.Role = value;
            FindEngineers();
        }

        private void LevelChanged(EngineerExperience value)
        {
            if (value == EngineerExperience.None)
                Task.Level = null;
            else
                Task.Level = value;
            FindEngineers();
        }

        private void FindEngineers()
        {
            if (Task.Level != null && Task.Role != null)
            {
                var temp = s_bl?.Engineer.ReadAll(e => e.Role == Task.Role && e.Level >= Task.Level);
                EngineersList = temp == null ? new() : new(temp!);
            }
            else
                EngineersList = new();
        }
    }
}