using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class IssueReturnWindow : Window
    {
        public IssueReturnWindow()
        {
            InitializeComponent();
        }

        // Triggered the moment the window opens
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        // Fetches fresh data from the database and updates the UI
        private void RefreshData()
        {
            using (var db = new LibraryContext())
            {
                // 1. Load Members into the Dropdown
                MemberComboBox.ItemsSource = db.Members.ToList();

                // 2. Load only AVAILABLE books into the Issue grid
                AvailableBooksGrid.ItemsSource = db.Books.Where(b => b.IsAvailable).ToList();

                // 3. Load ACTIVE loans (where ReturnDate is null) into the Return grid
                // .Include() ensures we pull the related Book and Member names, not just IDs
                ActiveLoansGrid.ItemsSource = db.BookLoans
                                                .Include(l => l.Book)
                                                .Include(l => l.Member)
                                                .Where(l => l.ReturnDate == null)
                                                .ToList();
            }
        }

        private void IssueBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedMember = MemberComboBox.SelectedItem as Member;
            var selectedBook = AvailableBooksGrid.SelectedItem as Book;

            if (selectedMember == null || selectedBook == null)
            {
                IssueStatusText.Text = "⚠️ Please select both a borrower and a book.";
                IssueStatusText.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            using (var db = new LibraryContext())
            {
                var service = new LibraryService(db);
                bool success = service.BorrowBook(selectedMember.Id, selectedBook.Id);

                if (success)
                {
                    IssueStatusText.Text = "✅ Book successfully issued!";
                    IssueStatusText.Foreground = new SolidColorBrush(Colors.Green);
                    RefreshData(); // Immediately remove the book from the available list
                }
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedLoan = ActiveLoansGrid.SelectedItem as BookLoan;

            if (selectedLoan == null)
            {
                ReturnStatusText.Text = "⚠️ Please select a loan to return.";
                ReturnStatusText.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            using (var db = new LibraryContext())
            {
                var service = new LibraryService(db);
                decimal fine = service.ReturnBook(selectedLoan.Id);

                if (fine > 0)
                {
                    MessageBox.Show($"Book returned late! \n\nPlease collect an overdue fine of ${fine:0.00}.", "Overdue Fine", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                ReturnStatusText.Text = "✅ Book successfully returned!";
                ReturnStatusText.Foreground = new SolidColorBrush(Colors.Green);
                RefreshData(); // Move the book back to the available list
            }
        }
    }
}