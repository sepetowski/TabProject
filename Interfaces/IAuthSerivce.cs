using TabProjectServer.Models.DTO.Auth;

namespace TabProjectServer.Interfaces
{
    public interface IAuthSerivce
    {
        Task<UserRegisterResDTO?> CreateNewUserAsync(UserRegisterReqDTO req);
        Task<UserLoginResDTO?> LoginUserAsync(UserLoginReqDTO req);
        Task<RefreshTokenResDTO?> GenerateRefreshTokenAsync(RefreshTokenReqDTO req);
        Task<GetAllUsersResDTO> GetAllUsersAsync();
        Task<UpdateUserResDTO?> UpdateUserAsync(Guid id,UpdateUserReqDTO req);
    }
}
