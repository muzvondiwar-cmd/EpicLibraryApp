using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class ManageMembersWindow : Window
    {
        public ManageMembersWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshMemberGrid();
        }

        private void RefreshMemberGrid()
        {
            using (var db = new LibraryContext())
            {
                // Fetch all members and bind them to the DataGrid
                MembersGrid.ItemsSource = db.Members.ToList();
            }
        }

        private void RegisterMember_Click(object sender, RoutedEventArgs e)
        {
            string newName = NewMemberNameBox.Text.Trim();
            string selectedRole = ((ComboBoxItem)NewMemberRoleBox.SelectedItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(newName))
            {
                RegisterStatusText.Text = "⚠️ Please enter a name.";
                RegisterStatusText.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            using (var db = new LibraryContext())
            {
                // Create the new member object
                var newMember = new Member
                {
                    Name = newName,
                    Role = selectedRole
                };

                // Save to the SQLite database
                db.Members.Add(newMember);
                db.SaveChanges();

                RegisterStatusText.Text = $"✅ {newName} registered!";
                RegisterStatusText.Foreground = new SolidColorBrush(Colors.Green);
                
                // Clear the text box for the next entry
                NewMemberNameBox.Clear(); 
                
                // Refresh the grid to show the new person instantly
                RefreshMemberGrid(); 
            }
        }
    }
}