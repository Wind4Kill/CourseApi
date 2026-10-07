using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using CourseApi.Application.Authentication.DTOs;
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> FindUser(string email);
        Task RegisterUser(UserRegisterDto userCredentials, CancellationToken cancellationToken);

        Task<TokensBearerDto> RefreshTokens(string refreshToken);
        Task<TokensBearerDto> LoginUser(UserLoginDto userCredentials, CancellationToken cancellationToken);
    }
}