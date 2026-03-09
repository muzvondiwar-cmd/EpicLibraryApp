using System;
using System.Linq;
using System.Windows;
using EpicLibraryApp.Data;

namespace EpicLibraryApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new LibraryContext())
            {
                db.Database.EnsureCreated();

                if (!db.Members.Any())
                {
                    db.Members.AddRange(
                        new Member { StudentId = "admin", Password = "adminpassword", Name = "System Admin", Role = "Staff" },
                        new Member { StudentId = "250591", Password = "password123", Name = "Engineering Student", Role = "Student" }
                    );
                    db.SaveChanges();
                }

                if (!db.Books.Any())
                {
                    db.Books.AddRange(
                        new Book { Title = "Advanced Engineering Mathematics", Author = "Erwin Kreyszig", ISBN = "978-0470458365", IsAvailable = true },
                        new Book { Title = "Clean Architecture", Author = "Robert C. Martin", ISBN = "978-0134494166", IsAvailable = true },
                        new Book { Title = "Engineering Mechanics: Dynamics", Author = "J.L. Meriam", ISBN = "978-1118885840", IsAvailable = true },
                        new Book { Title = "Design Patterns: Elements of Reusable Object-Oriented Software", Author = "Erich Gamma", ISBN = "978-0201633610", IsAvailable = true }
                    );
                    db.SaveChanges();
                }
            }
        }
    }
}