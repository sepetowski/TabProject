using Microsoft.EntityFrameworkCore;
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

     

        public BooksService(DataContext context) {

            _context = context;
        }

        public async Task<GetAllBooksResDTO> GetAllBooksAsync()
        {
            var books = await _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Categories)
                        .ToListAsync();

            var booksWithAuthorsAndCategories = books.Select(book => new BookWithAuthorAndCategoriesDTO
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.Author.Id,
                AuthorName = book.Author.Name,
                AuthorSurname = book.Author.Surname,
                Categories = book.Categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList()
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
               .FirstOrDefaultAsync(b => b.Id == id);

         
            if (book == null)
                return null;


            var bookDetails = new GetBookDetailsResDTO
            {
                Id = book.Id,
                Title = book.Title,
                NumberOfPage = book.NumberOfPage,
                PublicationDate = book.PublicationDate,
                AuthorId = book.Author.Id,
                AuthorName = book.Author.Name,
                AuthorSurname = book.Author.Surname,
                AuthorDescription = book.Author.Description,
                AuthorDateOfBirth = book.Author.DateOfBirth,
                BookDescripton= book.BookDescripton,
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

          

            var newBook = new Book
            {
                Id = Guid.NewGuid(),
                AuthorId = bookReqDTO.AuthorId,
                Title = bookReqDTO.Title,
                NumberOfPage = bookReqDTO.NumberOfPage,
                PublicationDate = bookReqDTO.PublicationDate,
                Author = existingAuthor,
                BookDescripton= bookReqDTO.BookDescripton,
                Categories = new List<Category>()
            };


            foreach (var category in categories)
            {
                newBook.Categories.Add(category);
            }

           

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var res = new AddBookResDTO
            {
                Id = newBook.Id
            };

            return res;
        }

        public async Task<UpdateBookResDTO?> UpdateBookAsync(Guid id, UpdateBookReqDTO req)
        {
            var book = await _context.Books
              .Include(b => b.Author)
              .Include(b => b.Categories)
              .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new Exception("Book not found");


            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == req.Title && b.AuthorId == book.AuthorId && b.Id != id);


            if (existingBook != null) throw new Exception("This book already exist");


            book.Title = req.Title;
            book.BookDescripton = req.BookDescripton;
            book.NumberOfPage = req.NumberOfPage;
            book.PublicationDate = req.PublicationDate;


            var categories = await _context.Categories.Where(c => req.CategoriesIds.Contains(c.Id)).ToListAsync();


            book.Categories.Clear();

            
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
    }
}
