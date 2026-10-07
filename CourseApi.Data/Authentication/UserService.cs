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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Authentication
{
    public class UserService(UserManager<User> userManager, ApplicationContext dbContext, ITokenProvider tokenProvider) : IUserService
    {
        public async Task RegisterUser(UserRegisterDto userCredentials, CancellationToken cancellationToken)
        {
            try

            {
                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await dbContext.Database.BeginTransactionAsync();

                    User createdUser = new User(userCredentials.UserName)
                    {
                        Email = userCredentials.Email
                    };

                    await userManager.CreateAsync(createdUser, userCredentials.Password);

                    List<Claim> userClaims = new List<Claim>
                    {
                    new Claim("Role", "User")
                    };

                    await userManager.AddClaimsAsync(createdUser, userClaims);
                    await transaction.CommitAsync();
                });

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<string> LoginUser(UserLoginDto userCredential, CancellationToken cancellationToken)
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

            List<Claim> claims = (await userManager.GetClaimsAsync(requestedUser)).ToList();

            claims.AddRange([
                new Claim("Id", $"{requestedUser.Id}"),
                new Claim("Email", $"{requestedUser.Email}"),
                new Claim("UserName", $"{requestedUser.UserName}")
                ]);

            string accessToken = tokenProvider.CreateAccessToken(claims);

            return accessToken;
        }
    }
}