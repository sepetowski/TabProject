using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Books;
using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Services
{
    public class BooksService : IBooksService
    {

        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;



        public BooksService(DataContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<GetAllBooksResDTO> GetAllBooksAsync()
        {
            var books = await _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Categories)
                        .Include(b => b.Reservations)
                        .ToListAsync();

            var booksWithAuthorsAndCategories = books.Select(book =>
            {
              
                var activeReservationsCount = book.Reservations.Count(r => r.IsActive);

              
                var isAvaible = book.AvailableCopies > 0 && activeReservationsCount < book.AvailableCopies;

                return new BookWithAuthorAndCategoriesDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorId = book.Author.Id,
                    AuthorName = book.Author.Name,
                    AuthorSurname = book.Author.Surname,
                    ImageUrl = book.ImageUrl,
                    isAvaible = isAvaible,
                    Categories = book.Categories.Select(category => new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name
                    }).ToList()
                };
            }).ToList();


            return new GetAllBooksResDTO
            {
                Books = booksWithAuthorsAndCategories,
                Amount= booksWithAuthorsAndCategories.Count()
            };
        }

        public async Task<GetBookDetailsResDTO?> GetBookDetailsAsync(Guid id)
        {
            var book = await _context.Books
               .Include(b => b.Author)
               .Include(b => b.Categories)
               .Include(b => b.Reservations)
               .FirstOrDefaultAsync(b => b.Id == id);

         
            if (book == null)
                return null;

            var activeReservationsCount = book.Reservations.Count(r => r.IsActive);

            var isAvaible = book.AvailableCopies > 0 && activeReservationsCount < book.AvailableCopies;


            var bookDetails = new GetBookDetailsResDTO
            {
                Id = book.Id,
                Title = book.Title,
                NumberOfPage = book.NumberOfPage,
                PublicationDate = book.PublicationDate,
                AuthorId = book.Author.Id,
                AuthorName = book.Author.Name,
                AuthorSurname = book.Author.Surname,
                ImageUrl= book.ImageUrl,
                AuthorDescription = book.Author.Description,
                AuthorDateOfBirth = book.Author.DateOfBirth,
                BookDescripton= book.BookDescripton,
                isAvaible= isAvaible,
                availableCopies= book.AvailableCopies,
                Categories = book.Categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList()
            };

            return bookDetails;

        }
        public async Task<AddBookResDTO?> AddBookAsync(AddBookReqDTO bookReqDTO)
        {

            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(author=>author.Id==bookReqDTO.AuthorId);

            if (existingAuthor == null)
                throw new Exception("Author not found");

            var existingBook=  await _context.Books.FirstOrDefaultAsync(book=>book.Title==bookReqDTO.Title && book.AuthorId== existingAuthor.Id);

            if (existingBook != null)
                throw new Exception("Book already exists");


            var categories = await _context.Categories.Where(c => bookReqDTO.CategoriesIds.Contains(c.Id)).ToListAsync();


            var bookId = Guid.NewGuid();

            var newBook = new Book
            {
                Id = bookId,
                AuthorId = bookReqDTO.AuthorId,
                Title = bookReqDTO.Title,
                NumberOfPage = bookReqDTO.NumberOfPage,
                PublicationDate = bookReqDTO.PublicationDate,
                Author = existingAuthor,
                BookDescripton = bookReqDTO.BookDescripton,
                Categories = new List<Category>(),
                AvailableCopies = bookReqDTO.AvailableCopies,
            };


            if (bookReqDTO.CategoriesIds != null)
                foreach (var category in categories)
            {
                newBook.Categories.Add(category);
            }



            if (bookReqDTO.ImageFile != null)
            {

                var urlFilePath = await UpdateBookImageAsync(bookId, bookReqDTO.ImageFile);
                newBook.ImageUrl = urlFilePath;
            }



            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var res = new AddBookResDTO
            {
                Id = newBook.Id
            };

            return res;
        }

        public async Task<UpdateBookResDTO?> UpdateBookAsync(UpdateBookReqDTO req)
        {
            var book = await _context.Books
              .Include(b => b.Author)
              .Include(b => b.Categories)
              .FirstOrDefaultAsync(b => b.Id == req.Id);

            if (book == null)
                throw new Exception("Book not found");


            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == req.Title && b.AuthorId == book.AuthorId && b.Id != req.Id);


            if (existingBook != null) throw new Exception("This book already exist");


            

            book.Title = req.Title;
            book.BookDescripton = req.BookDescripton;
            book.NumberOfPage = req.NumberOfPage;
            book.PublicationDate = req.PublicationDate;
            book.AvailableCopies = req.AvailableCopies;

            if (req.ImageFile != null)
            {

                var urlFilePath = await UpdateBookImageAsync(book.Id, req.ImageFile);
                book.ImageUrl = urlFilePath;
            }
            else if(req.ImageFile == null && req.DeleteFile) 
            {
                
                string imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
                var existingFiles = Directory.GetFiles(imagesDirectory, $"{book.Id}.*");

                foreach (var f in existingFiles)
                {
                    File.Delete(f);
                }

                book.ImageUrl = null;
            }


            var categories = await _context.Categories.Where(c => req.CategoriesIds.Contains(c.Id)).ToListAsync();


            book.Categories.Clear();

            if(req.CategoriesIds !=null)
            foreach (var category in categories)
            {
                book.Categories.Add(category);
            }

            await _context.SaveChangesAsync();

            return new UpdateBookResDTO
            {
                Id = book.Id,
                Title = book.Title,
                BookDescripton = book.BookDescripton,
                NumberOfPage = book.NumberOfPage,
                PublicationDate = book.PublicationDate,
                ImageUrl= book.ImageUrl,
                AvailableCopies=book.AvailableCopies,
                Categories = book.Categories.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };

        }


       


        public async Task<DeleteBookResDTO?> DeleteBookResAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return null;

            _context.Books.Remove(book);

        
            await _context.SaveChangesAsync();

            return new DeleteBookResDTO
            {
                Id = book.Id
            };

        }


        private async Task<string> UpdateBookImageAsync(Guid bookId, IFormFile file)
        {

            string fileExtension = Path.GetExtension(file.FileName).ToLower();


            string imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            string localPath = Path.Combine(imagesDirectory, $"{bookId}{fileExtension}");


            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);


            var existingFiles = Directory.GetFiles(imagesDirectory, $"{bookId}.*");
            foreach (var f in existingFiles)
            {
                File.Delete(f);
            }


            

            using var stream = new FileStream(localPath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(stream);


            var contextRequest = _contextAccessor.HttpContext.Request;
            var urlFilePath = $"{contextRequest.Scheme}://{contextRequest.Host}/Images/{bookId}{fileExtension}";

            return urlFilePath;

        }
    }
}
