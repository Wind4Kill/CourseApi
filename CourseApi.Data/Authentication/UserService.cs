using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using CourseApi.Application.Authentication.DTOs;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Authentication
{
    public class UserService(UserManager<User> userManager, ApplicationContext dbContext) : IUserService
    {
        public Task LoginUser(UserLoginDto userCredentials, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterUser(UserRegisterDto userCredentials, CancellationToken cancellationToken)
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
                new Claim("Role","User")
            };

                await userManager.AddClaimsAsync(createdUser, userClaims);
                await transaction.CommitAsync();
            });

        }
    }
}