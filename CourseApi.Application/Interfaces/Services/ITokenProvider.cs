using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Services
{
    public interface ITokenProvider
    {
        string CreateAccessToken(List<Claim> userClaims);
        string CreateRefreshToken();
    }
}