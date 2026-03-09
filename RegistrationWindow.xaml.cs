using System.Linq;
using System.Windows;
using System.Windows.Media;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            string studentId = StudentIdBox.Text.Trim();
            string password = PasswordBox.Password;

            // 1. Basic validation
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("⚠️ Please fill in all fields.", Colors.DarkOrange);
                return;
            }

            using (var db = new LibraryContext())
            {
                // 2. Check if the Student ID is already registered
                bool userExists = db.Members.Any(m => m.StudentId == studentId);
                
                if (userExists)
                {
                    ShowMessage("❌ This Student ID is already registered.", Colors.Red);
                    return;
                }

                // 3. Create the new user
                var newStudent = new Member
                {
                    Name = fullName,
                    StudentId = studentId,
                    Password = password,
                    Role = "Student" // Automatically force the role to Student for security
                };

                // 4. Save to database
                db.Members.Add(newStudent);
                db.SaveChanges();

                ShowMessage("✅ Registration successful! You can now log in.", Colors.Green);
                
                // Clear the form
                FullNameBox.Clear();
                StudentIdBox.Clear();
                PasswordBox.Clear();
            }
        }

        private void ShowMessage(string message, Color color)
        {
            StatusMessage.Text = message;
            StatusMessage.Foreground = new SolidColorBrush(color);
            StatusMessage.Visibility = Visibility.Visible;
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}