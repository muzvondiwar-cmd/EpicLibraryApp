using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore; 
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

            // 1. Personalize the text based on who logged in
            WelcomeMessageText.Text = $"Welcome back, {_loggedInUser.Name}";
            UserRoleText.Text = $"Logged in as: {_loggedInUser.Role} (ID: {_loggedInUser.StudentId}) | Africa University Engineering Library";

            // 2. ROLE-BASED ACCESS CONTROL (Security)
            if (_loggedInUser.Role == "Student")
            {
                // Hide staff-only tools from students
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
                // Update Summary Cards
                int totalBooksCount = db.Books.Count();
                int activeLoansCount = db.BookLoans.Count(loan => loan.ReturnDate == null);

                TotalBooksText.Text = totalBooksCount.ToString();
                ActiveLoansText.Text = activeLoansCount.ToString();

                // Fetch "My Books" for the specific logged-in user
                var myActiveLoans = db.BookLoans
                                      .Include(l => l.Book) 
                                      .Where(l => l.MemberId == _loggedInUser.Id && l.ReturnDate == null)
                                      .ToList();

                // Bind the data to the grid
                MyBooksGrid.ItemsSource = myActiveLoans;
            }
        }

        private void OpenCatalog_Click(object sender, RoutedEventArgs e)
        {
            // Open the catalog and pass the current user's data
            StudentCatalogWindow catalog = new StudentCatalogWindow(_loggedInUser);
            catalog.ShowDialog();
            
            // Refresh stats after the window closes
            RefreshDashboardStats();
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
            // Log out and return to the secure login screen
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}