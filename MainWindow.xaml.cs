using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore; // Added this!
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

            if (_loggedInUser.Role == "Student")
            {
                IssueReturnButton.Visibility = Visibility.Collapsed;
                ManageMembersButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDashboardStats();
        }

        private void RefreshDashboardStats()
        {
            using (var db = new LibraryContext())
            {
                // 1. Update the Summary Cards
                int totalBooksCount = db.Books.Count();
                int activeLoansCount = db.BookLoans.Count(loan => loan.ReturnDate == null);

                TotalBooksText.Text = totalBooksCount.ToString();
                ActiveLoansText.Text = activeLoansCount.ToString();

                // 2. Fetch "My Books" for the specific logged-in user
                var myActiveLoans = db.BookLoans
                                      .Include(l => l.Book) // Grab the actual book details so we can show the title
                                      .Where(l => l.MemberId == _loggedInUser.Id && l.ReturnDate == null)
                                      .ToList();

                // 3. Bind the data to the new grid
                MyBooksGrid.ItemsSource = myActiveLoans;
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