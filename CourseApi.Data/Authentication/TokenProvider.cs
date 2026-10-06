using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using CourseApi.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CourseApi.Data.Authentication
{
    public class TokenProvider(IOptions<JwtTokenSettings> options) : ITokenProvider
    {
        public string CreateAccessToken(List<Claim> userClaims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = signingCredentials
            };

            string accessToken = new JsonWebTokenHandler().CreateToken(descriptor);
            return accessToken;
        }

        public string CreateRefreshToken()
        {
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            return refreshToken;
        }
    }
}