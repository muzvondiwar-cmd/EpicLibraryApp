using Microsoft.EntityFrameworkCore;

namespace EpicLibraryApp.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // CHANGED: We are now using SQL Server instead of SQLite!
            // This string tells the app exactly where the server is and how to log in.
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=AfricaUniLibraryDb_V2;Trusted_Connection=True;TrustServerCertificate=True;";            
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}