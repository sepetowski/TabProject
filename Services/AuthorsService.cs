using Microsoft.EntityFrameworkCore;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Authors;

namespace TabProjectServer.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly DataContext _context;


        public AuthorsService(DataContext context) { 
            _context = context;
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
    }
}
