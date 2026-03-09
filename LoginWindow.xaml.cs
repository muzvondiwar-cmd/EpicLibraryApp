using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredId = UsernameTextBox.Text.Trim();
            string enteredPassword = PasswordBox.Password;
            string selectedRole = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            using (var db = new LibraryContext())
            {
                // 1. Query the database for a matching user
                var validUser = db.Members.FirstOrDefault(m => 
                    m.StudentId == enteredId && 
                    m.Password == enteredPassword && 
                    m.Role == selectedRole);

                // 2. If the user exists and credentials are correct
                if (validUser != null)
                {
                    ErrorMessage.Visibility = Visibility.Collapsed;

                    // Open the main dashboard and pass the valid user data
                    MainWindow dashboard = new MainWindow(validUser);
                    dashboard.Show();

                    // Close the login screen securely
                    this.Close();
                }
                else
                {
                    // Show error message
                    ErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }

        private void OpenRegistration_Click(object sender, RoutedEventArgs e)
        {
            // Open the new Registration screen
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            
            // Close the login screen
            this.Close();
        }
    }
}