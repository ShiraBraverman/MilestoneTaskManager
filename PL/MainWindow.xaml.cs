using PL.Engineer;
using PL.Task;
using System.Windows;
using System.Windows.Input.Manipulations;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // פונקציה המתבצעת כאשר לוחצים על כפתור "מנהל"
    private void BtnManeger_Click(object sender, RoutedEventArgs e)
    {
        // בקשה מהמשתמש להזין את המזהה שלו
        string userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter your Id:", "Enter Id", "248728764");

        // בדיקה אם המשתמש הזין ערך ולא עזב אותו ריק
        if (!string.IsNullOrEmpty(userInput))
        {
            // פתיחת חלון המנהל
            new ManagerWindow().Show();
        }
    }

    // פונקציה המתבצעת כאשר לוחצים על כפתור "מהנדס"
    private void BtnEngineer_Click(object sender, RoutedEventArgs e)
    {
        // בקשה מהמשתמש להזין את המזהה שלו
        string userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter your Id:", "Enter Id", "165324683");

        // בדיקה אם המשתמש הזין ערך ולא עזב אותו ריק
        if (!string.IsNullOrEmpty(userInput))
        {
            // פתיחת חלון המהנדס עם המזהה שהמשתמש הזין
            new EngineerWindow(int.Parse(userInput)).Show();
        }
    }

    // בנאי של החלון הראשי
    public MainWindow()
    {
        InitializeComponent();
    }
}