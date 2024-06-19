using Microsoft.EntityFrameworkCore;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Reservations;

namespace TabProjectServer.Services
{
    public class ReservationsService : IReservationsService
    {


        private readonly DataContext _context;

        public ReservationsService(DataContext context)
        {
            _context = context;
        }

        public async Task<CancelReservationResDTO?> CancelReservationAsync(CancelReservationReqDTO req)
        {

            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == req.Id);


            if (reservation == null)
                return null;
            

            reservation.IsActive = false;
            await _context.SaveChangesAsync();

           
            return new CancelReservationResDTO
            {
                Id = reservation.Id,
                ReservationDate = reservation.ReservationDate
            };

        }

        public async Task<CreateReservationResDTO?> CreateReservationAsync(CreateReservationReqDTO req)
        {

            var user = await _context.Users.FindAsync(req.UserId);

            if (user == null) 
                throw new Exception("User not found");

            var book = await _context.Books.FindAsync(req.BookId);

            if(book == null)
                throw new Exception("Book not found");

            var activeLoan = await _context.Loans.FirstOrDefaultAsync(l => l.UserId == req.UserId && l.BookId == req.BookId && l.ReturnDate == null);
            if (activeLoan != null)
                throw new Exception("You already have an active loan for this book.");


            var activeReservationsCount = await _context.Reservations.CountAsync(r => r.BookId == book.Id && r.IsActive);

            if (book.AvailableCopies > 0 && activeReservationsCount < book.AvailableCopies)
                throw new Exception("You can just loan this book");

            var existingReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.UserId == req.UserId && r.BookId == req.BookId && r.IsActive);
            if (existingReservation != null)
                throw new Exception("You already have an active reservation for this book.");

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId,
                BookId = req.BookId,
                ReservationDate = DateTime.Now,
                IsActive = true,
                Book = book,
                User =user

                
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return new CreateReservationResDTO { 
                Id = reservation.Id,
                ReservationDate=reservation.ReservationDate 
            };
        }

        public async Task<GetAllReservationsResDTO> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Book)
                        .ThenInclude(b => b.Author)
                    .ToListAsync();

            var reservationDTOs = reservations.Select(reservation => new ReservationDTO
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                Username = reservation.User.Username,
                BookId = reservation.BookId,
                BookTitle = reservation.Book.Title,
                BookAuthorName = reservation.Book.Author.Name,
                BookAuthorSurnameName = reservation.Book.Author.Surname,
                ReservationDate = reservation.ReservationDate,
                IsActive = reservation.IsActive,
                ImageUrl = reservation.Book.ImageUrl
            }).ToList();

            return new GetAllReservationsResDTO
            {
                Reservations = reservationDTOs,
                Amount = reservationDTOs.Count
            };
        }

        public async Task<GetUserReservationsResDTO?> GetUserReservationsAsync(Guid id)
        {
            var reservations = await _context.Reservations
                   .Include(r => r.User)
                   .Include(r => r.Book)
                       .ThenInclude(b => b.Author)
                       .Where(u => u.User.Id == id)
                   .ToListAsync();

            if (reservations == null) return null;

            var reservationDTOs = reservations.Select(reservation => new ReservationDTO
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                Username = reservation.User.Username,
                BookId = reservation.BookId,
                BookTitle = reservation.Book.Title,
                BookAuthorName = reservation.Book.Author.Name,
                BookAuthorSurnameName = reservation.Book.Author.Surname,
                ReservationDate = reservation.ReservationDate,
                IsActive = reservation.IsActive,
                ImageUrl= reservation.Book.ImageUrl
            }).ToList();

            return new GetUserReservationsResDTO
            {
                Reservations = reservationDTOs,
                Amount = reservationDTOs.Count
            };
        }
    }
}
