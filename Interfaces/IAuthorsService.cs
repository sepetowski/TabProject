using TabProjectServer.Models.DTO.Authors;

namespace TabProjectServer.Interfaces
{
    public interface IAuthorsService
    {
        Task<AddAuthorResDTO?> CreateAuthorAsync(AddAuthorReqDTO req);
        Task <GetAllAuthorsResDTO> GetAllAuthorsAsync();
        Task<GetAuthorDetailsResDTO?> GetAuthorDetailsAsync(Guid id);
        Task <UpdateAuthorResDTO?> UpdateAuthorAsync(Guid id, UpdateAuthorReqDTO req);
        Task <DeleteAuthorResDTO?> DeleteAuthorAsync(Guid id);
    }
}
