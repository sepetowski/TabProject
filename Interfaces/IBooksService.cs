
using TabProjectServer.Models.DTO.Books;

namespace TabProjectServer.Interfaces
{
    public interface IBooksService
    {

        Task<AddBookResDTO?> AddBookAsync(AddBookReqDTO req);
        Task<GetAllBooksResDTO> GetAllBooksAsync();
        Task<GetBookDetailsResDTO?> GetBookDetailsAsync(Guid id);
        Task<DeleteBookResDTO?> DeleteBookResAsync(Guid id);

        Task<UpdateBookResDTO?> UpdateBookAsync(Guid id,UpdateBookReqDTO req);
    }
}
