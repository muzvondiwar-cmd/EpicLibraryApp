using System;
using System.Linq;

namespace EpicLibraryApp.Data
{
    public class LibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }

        public bool BorrowBook(int memberId, int bookId, int daysToBorrow = 14)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            
            if (book != null && book.IsAvailable)
            {
                book.IsAvailable = false; 
                var newLoan = new BookLoan
                {
                    BookId = bookId,
                    MemberId = memberId,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(daysToBorrow), 
                    FineAmount = 0
                };

                _context.BookLoans.Add(newLoan);
                _context.SaveChanges();
                return true; 
            }
            return false; 
        }

        public decimal ReturnBook(int loanId)
        {
            var loan = _context.BookLoans.FirstOrDefault(l => l.Id == loanId);
            
            if (loan != null && loan.ReturnDate == null)
            {
                loan.ReturnDate = DateTime.Now;
                
                var book = _context.Books.FirstOrDefault(b => b.Id == loan.BookId);
                if (book != null) 
                {
                    book.IsAvailable = true;
                }

                if (loan.ReturnDate > loan.DueDate)
                {
                    TimeSpan lateBy = loan.ReturnDate.Value - loan.DueDate;
                    int lateDays = (int)Math.Ceiling(lateBy.TotalDays); 
                    loan.FineAmount = lateDays * 2.0m; 
                }

                _context.SaveChanges();
                return loan.FineAmount; 
            }
            return 0; 
        }
    }
}