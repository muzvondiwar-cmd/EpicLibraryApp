using System.Linq;
using System.Windows;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class MainWindow : Window
    {
        private Member _loggedInUser;

        public MainWindow(Member user)
        {
            InitializeComponent();
            _loggedInUser = user;

            WelcomeMessageText.Text = $"Welcome back, {_loggedInUser.Name}";
            UserRoleText.Text = $"Logged in as: {_loggedInUser.Role} (ID: {_loggedInUser.StudentId}) | Africa University Engineering Library";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDashboardStats();
        }

        private void RefreshDashboardStats()
        {
            using (var db = new LibraryContext())
            {
                int totalBooksCount = db.Books.Count();
                int activeLoansCount = db.BookLoans.Count(loan => loan.ReturnDate == null);

                TotalBooksText.Text = totalBooksCount.ToString();
                ActiveLoansText.Text = activeLoansCount.ToString();
            }
        }

        private void OpenIssueReturn_Click(object sender, RoutedEventArgs e)
        {
            IssueReturnWindow issueWindow = new IssueReturnWindow();
            issueWindow.ShowDialog(); 
            RefreshDashboardStats();
        }

        private void OpenMembers_Click(object sender, RoutedEventArgs e)
        {
            ManageMembersWindow membersWindow = new ManageMembersWindow();
            membersWindow.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}