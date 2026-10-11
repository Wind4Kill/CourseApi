using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Application.Authentication
{
    public record UserRegisterDto(string Email, string UserName, string Password);
}