using System;

namespace EpicLibraryApp.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class Member
    {
        public int Id { get; set; } 
        public string? StudentId { get; set; } // Added for Login
        public string? Password { get; set; }  // Added for Login
        public string? Name { get; set; }
        public string? Role { get; set; } 
    }

    public class BookLoan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        
        public decimal FineAmount { get; set; }
    }
}