﻿using TabProjectServer.Models.DTO.Auth;

namespace TabProjectServer.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserRegisterResDTO?> CreateNewUserAsync(UserRegisterReqDTO req);
        Task<UserLoginResDTO?> LoginUserAsync(UserLoginReqDTO req);
        Task<RefreshTokenResDTO?> GenerateRefreshTokenAsync(RefreshTokenReqDTO req);
    }
}