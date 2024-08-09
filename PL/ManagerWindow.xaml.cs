using DalTest;
using PL.Task;
using PL.Engineer;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for ManagerWindow.xaml
/// </summary>
public partial class ManagerWindow : Window
{
    // פונקציה המתבצעת כאשר לוחצים על כפתור "רשימת משימות"
    private void BtnTaskList_Click(object sender, RoutedEventArgs e)
    {
        // פתיחת חלון רשימת המשימות
        new TaskListWindow().Show();
    }

    // פונקציה המתבצעת כאשר לוחצים על כפתור "רשימת מהנדסים"
    private void BtnEngineerList_Click(object sender, RoutedEventArgs e)
    {
        // פתיחת חלון רשימת המהנדסים
        new EngineerListWindow().Show();
    }

    // פונקציה המתבצעת כאשר לוחצים על כפתור "אתחול"
    private void BtnInitialization_Click(object sender, RoutedEventArgs e)
    {
        // בקשה מהמשתמש אם הוא רוצה ליצור נתונים ראשוניים
        MessageBoxResult result = MessageBox.Show("Would you like to create Initial data?", "Confirmation", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            // הרצת פונקציה ליצירת נתונים ראשוניים
            Initialization.Do();
        }
    }

    // בנאי של חלון המנהל
    public ManagerWindow()
    {
        InitializeComponent();
    }
}