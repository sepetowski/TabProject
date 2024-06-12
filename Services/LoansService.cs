using Microsoft.EntityFrameworkCore;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Loan;

namespace TabProjectServer.Services
{
    public class LoansService : ILoansService
    {

        private readonly DataContext _context;
     

        public LoansService(DataContext context)
        {
            _context = context;
        
        }

        public async Task<CreateLoanResDTO?> CreateLoanAsync(CreateLoanReqDTO req)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == req.BookId);

            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id == req.UserId);

            if (book == null)
                throw new Exception("Book not found");


            if (user == null)
                throw new Exception("User not found");



            var alreadyLoaned = await _context.Loans.AnyAsync(l => l.BookId == req.BookId && l.UserId == req.UserId && l.ReturnDate == null);
            if (alreadyLoaned)
                throw new Exception("You have already loaned this book");

            if(book.AvailableCopies==0)
                throw new Exception("No books avaible");

            var activeReservationsCount = await _context.Reservations.CountAsync(r => r.BookId == book.Id && r.IsActive);

            var userReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.BookId == book.Id && r.UserId == user.Id && r.IsActive);

            bool needReservation = activeReservationsCount >= book.AvailableCopies;

            
            if (needReservation && userReservation == null)
                throw new Exception("Book are reserved. Please make a reservation");


            if (needReservation)
            {

            var reservations = await _context.Reservations
                    .Where(r => r.BookId == book.Id && r.IsActive)
                    .OrderBy(r => r.ReservationDate)
                    .Take(book.AvailableCopies) 
                    .ToListAsync();

            var canUserLoand = reservations.Any(r => r.UserId == user.Id);

            if (!canUserLoand)
                throw new Exception("You are in quaqe.");

            if (canUserLoand && userReservation != null)
                    userReservation.IsActive = false;

            }

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                BookId = book.Id,
                UserId = user.Id,
                LoanDate = DateTime.Now,
                Book = book,
                User = user
            };

            

            book.AvailableCopies -= 1;
            _context.Loans.Add(loan);
            _context.SaveChanges();

            return new CreateLoanResDTO { Id = loan.Id };
        }

        public async  Task<GetAllLoansResDTO?> GetAllLoansAsync()
        {
            var loans = await _context.Loans
           .Include(l => l.Book)
                  .ThenInclude(b => b.Author)
           .Include(l => l.User)
           .ToListAsync();


            var loanDTOs = loans.Select(loan => new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                Username = loan.User.Username,
                BookId = loan.BookId,
                BookTitle = loan.Book.Title,
                BookAuthor = loan.Book.Author.Name, 
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            }).ToList();

            var response = new GetAllLoansResDTO
            {
                Loans = loanDTOs,
                Amount = loanDTOs.Count
            };

            return response;
        }

        public async Task<GetUserLoansResDTO?> GetUserLoansAsync(Guid id)
        {
            var loans = await _context.Loans
            .Include(l => l.Book)
                .ThenInclude(b => b.Author)
            .Include(l => l.User)
            .Where(l => l.UserId == id)
            .ToListAsync();

            if (loans == null) return null;

            var loanDTOs = loans.Select(loan => new LoanDTO
            {
                Id = loan.Id,
                UserId = loan.UserId,
                Username = loan.User.Username,
                BookId = loan.BookId,
                BookTitle = loan.Book.Title,
                BookAuthor = loan.Book.Author.Name, 
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            }).ToList();

            var response = new GetUserLoansResDTO
            {
                Loans = loanDTOs,
                Amount = loanDTOs.Count
            };

            return response;

        }

        public async  Task<ReturnLoanResDTO?> ReturnLoanAsync(ReturnLoanReqDTO req)
        {
            var loan = await _context.Loans.Include(l=>l.Book).FirstOrDefaultAsync(l=>l.Id == req.Id);

            if (loan == null)
                throw new Exception("Loan not found");


            if (loan.ReturnDate != null)
                throw new Exception($"This book was returned on {loan.ReturnDate}");


            loan.ReturnDate = DateTime.Now;
            loan.Book.AvailableCopies += 1;


            await _context.SaveChangesAsync();

            var response = new ReturnLoanResDTO
            {
                Id = loan.Id,
                ReturnDate = (DateTime)loan.ReturnDate

            };


            return response;



        }
    }
}
