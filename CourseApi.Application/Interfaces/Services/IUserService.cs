using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using CourseApi.Application.Authentication.DTOs;

namespace CourseApi.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUser(UserRegisterDto userCredentials, CancellationToken cancellationToken);
        Task<string> LoginUser(UserLoginDto userCredentials, CancellationToken cancellationToken);
    }
}