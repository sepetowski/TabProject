using TabProjectServer.Models.DTO.Authors;

namespace TabProjectServer.Interfaces
{
    public interface IAuthorsService
    {
        Task<AddAuthorResDTO?> CreateAuthorAsync(AddAuthorReqDTO req);
    }
}
