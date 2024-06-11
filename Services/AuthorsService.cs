using Microsoft.EntityFrameworkCore;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Authors;
using TabProjectServer.Models.DTO.Books;
using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly DataContext _context;


        public AuthorsService(DataContext context) { 
            _context = context;
        }

        public async Task<GetAllAuthorsResDTO> GetAllAuthorsAsync() {

            var authors = await _context.Authors
                        .Select(a => new AuthorDTO
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Surname = a.Surname,
                             Description = a.Description,
                             DateOfBirth = a.DateOfBirth
                         }).ToListAsync();

          

            var response = new GetAllAuthorsResDTO
            {
                Authors = authors ?? [],
                Amount = authors?.Count ?? 0,
            };



            return response;
        }

        public async Task<GetAuthorDetailsResDTO?> GetAuthorDetailsAsync(Guid id)
        {
            var existingAuthor = await _context.Authors
                    .Include(author => author.Books)
                    .ThenInclude(book => book.Categories) 
                    .FirstOrDefaultAsync(author => author.Id == id);

            if (existingAuthor == null)
                return null;

            var bookDTOs = existingAuthor.Books.Select(book => new BookWithCategoriesDTO
            {
                Id = book.Id,
                Title = book.Title,
                Categories = book.Categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList()
            }).ToList();

            var authorDetailsDTO = new GetAuthorDetailsResDTO
            {
                Id = existingAuthor.Id,
                Name = existingAuthor.Name,
                Surname = existingAuthor.Surname,
                Description = existingAuthor.Description,
                DateOfBirth = existingAuthor.DateOfBirth,
                Books = bookDTOs,
                Amount = bookDTOs.Count
            };


            return authorDetailsDTO;


        }


        public async Task<UpdateAuthorResDTO?> UpdateAuthorAsync(Guid id, UpdateAuthorReqDTO req)
        {
            var existingAuthor = await _context.Authors
               .FirstOrDefaultAsync(a => a.Id == id);


            if (existingAuthor == null)
                return null;

            existingAuthor.Name= req.Name;
            existingAuthor.Surname= req.Surname;
            existingAuthor.Description=req.Description;
            existingAuthor.DateOfBirth= req.DateOfBirth;


            await _context.SaveChangesAsync();


            var res = new UpdateAuthorResDTO
            {
                Name = existingAuthor.Name,
                Surname = existingAuthor.Surname,
                Description = existingAuthor.Description,
                DateOfBirth = existingAuthor.DateOfBirth
            };

            return res;
        }

        public async  Task<AddAuthorResDTO?> CreateAuthorAsync(AddAuthorReqDTO req)
        {
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name == req.Name && a.Surname == req.Surname);


            if (existingAuthor != null)
                throw new Exception("This Author already exists in database");


            var newAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Surname = req.Surname,
                Description = req.Description,
                DateOfBirth = req.DateOfBirth,
                Books = new List<Book>()
            };

            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();


            var response = new AddAuthorResDTO
            {
                Id = newAuthor.Id,
                Name = newAuthor.Name,
                Surname = newAuthor.Surname,
                Description = newAuthor.Description,
                DateOfBirth = newAuthor.DateOfBirth
            };

            return response;
        }


        public async Task<DeleteAuthorResDTO?> DeleteAuthorAsync(Guid id)
        {
            var existingAuthor = await _context.Authors
               .FirstOrDefaultAsync(author=>author.Id==id);

            if (existingAuthor == null) return null;


            var deleted = new DeleteAuthorResDTO
            {
                Id = existingAuthor.Id,
                Name = existingAuthor.Name,
                Surname = existingAuthor.Surname,
                DateOfBirth = existingAuthor.DateOfBirth,
                Description = existingAuthor.Description
            };

    
            _context.Authors.Remove(existingAuthor);
          
            await _context.SaveChangesAsync();

            return deleted;
        }
    }
}

