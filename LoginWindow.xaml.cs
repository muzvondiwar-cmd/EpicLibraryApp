using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            using (var db = new EpicLibraryApp.Data.LibraryContext())
            {
                var validUser = db.Members.FirstOrDefault(m => 
                    m.StudentId == enteredId && 
                    m.Password == enteredPassword && 
                    m.Role == selectedRole);

                if (validUser != null)
                {
                    ErrorMessage.Visibility = Visibility.Collapsed;

                    MainWindow dashboard = new MainWindow(validUser);
                    dashboard.Show();

                    this.Close();
                }
                else
                {
                    ErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }
    }
}