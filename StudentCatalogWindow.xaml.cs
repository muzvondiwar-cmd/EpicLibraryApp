using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class StudentCatalogWindow : Window
    {
        private Member _loggedInStudent;

        // Require the logged-in user data when this window opens
        public StudentCatalogWindow(Member student)
        {
            InitializeComponent();
            _loggedInStudent = student;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAvailableBooks();
        }

        private void LoadAvailableBooks()
        {
            using (var db = new LibraryContext())
            {
                // Fetch only books that are currently available
                var availableBooks = db.Books.Where(b => b.IsAvailable).ToList();
                BooksItemsControl.ItemsSource = availableBooks;
            }
        }

        // This runs when they click the "Borrow Now" button on ANY card
        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {
            // 1. Figure out which specific card/book was clicked
            var button = sender as Button;
            var selectedBook = button.DataContext as Book;

            if (selectedBook != null)
            {
                using (var db = new LibraryContext())
                {
                    var service = new LibraryService(db);
                    
                    // 2. Issue the book directly to the logged-in student
                    bool success = service.BorrowBook(_loggedInStudent.Id, selectedBook.Id);

                    if (success)
                    {
                        MessageBox.Show($"Success! '{selectedBook.Title}' has been added to your active loans.", "Book Borrowed", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // 3. Refresh the grid so the borrowed book disappears from the catalog
                        LoadAvailableBooks(); 
                    }
                    else
                    {
                        MessageBox.Show("Sorry, this book is no longer available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}