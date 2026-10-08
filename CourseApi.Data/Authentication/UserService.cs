using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using CourseApi.Application.Authentication.DTOs;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
using CourseApi.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Authentication
{
    public class UserService(IHttpContextAccessor httpContext, UserManager<User> userManager, ApplicationContext dbContext, ITokenProvider tokenProvider) : IUserService
    {

        public async Task<User?> FindUser(string email)
        {
            User? requestedUser = await userManager.FindByEmailAsync(email);
            return requestedUser;
        }

        public async Task RegisterUser(UserRegisterDto userCredentials, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                try
                {

                    using var transaction = await dbContext.Database.BeginTransactionAsync();

                    User createdUser = new User(userCredentials.UserName)
                    {
                        Email = userCredentials.Email
                    };

                    var result = await userManager.CreateAsync(createdUser, userCredentials.Password);
                    if (!result.Succeeded)
                    {
                        throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }

                    List<Claim> userClaims = new List<Claim>
                    {
                            new Claim("Role", "User")
                    };

                    await userManager.AddClaimsAsync(createdUser, userClaims);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            });

        }

        public async Task<TokensBearerDto> LoginUser(UserLoginDto userCredential, CancellationToken cancellationToken)
        {
            User? requestedUser = await userManager.FindByEmailAsync(userCredential.Email);

            if (requestedUser is null)
            {
                throw new UserNotFoundException("User with provided email address wasn't found. Please, register first");
            }

            var result = await userManager.CheckPasswordAsync(requestedUser, userCredential.Password);

            if (!result)
            {
                throw new ValidationException("Provided user credentials are incorrect.");
            }

            TokensBearerDto tokensBearer = await GenerateTokens(requestedUser);


            RefreshToken refreshToken = new()
            {
                Expiration = DateTime.UtcNow.AddMinutes(30),
                Token = tokensBearer.RefreshToken,
                User = requestedUser
            };

            dbContext.RefreshTokens.Add(refreshToken);

            await dbContext.SaveChangesAsync();

            return tokensBearer;
        }

        public async Task<TokensBearerDto> RefreshTokens(string refreshToken)
        {
            RefreshToken? requestedToken = await dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);

            if (requestedToken is null || requestedToken.Expiration < DateTime.UtcNow)
            {
                throw new Exception("Token expired.");
            }

            await dbContext.Entry<RefreshToken>(requestedToken).Reference(c => c.User).LoadAsync();

            User requestedUser = requestedToken.User;

            if (!CheckUser(requestedToken.UserId))
            {
                throw new Exception("This operation is not permitted.");
            }

            await dbContext.RefreshTokens.Where(rt => rt.UserId == requestedToken.UserId).ExecuteDeleteAsync();

            TokensBearerDto tokensBearer = await GenerateTokens(requestedUser);

            requestedToken = new RefreshToken()
            {
                Expiration = DateTime.UtcNow.AddMinutes(30),
                Token = tokensBearer.RefreshToken,
                User = requestedUser
            };

            dbContext.RefreshTokens.Add(requestedToken);

            await dbContext.SaveChangesAsync();

            return tokensBearer;

        }

        private async Task<TokensBearerDto> GenerateTokens(User user)
        {
            List<Claim> claims = (await userManager.GetClaimsAsync(user)).ToList();

            claims.AddRange([new Claim("Id", user.Id), new Claim("UserName", user.UserName!), new Claim("Email", user.Email!)]);

            string newAccessToken = tokenProvider.CreateAccessToken(claims);

            string newRefreshTokenString = tokenProvider.CreateRefreshToken();

            TokensBearerDto tokensBearer = new()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString
            };

            return tokensBearer;
        }

        private bool CheckUser(string userId) => httpContext.HttpContext!.User.FindFirstValue("Id") == userId;
    }
}