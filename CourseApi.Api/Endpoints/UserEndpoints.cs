using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Api.EndpointFilters;
using CourseApi.Application.Authentication;
using CourseApi.Application.Authentication.DTOs;
using CourseApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CourseApi.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            var userEndpointsBuilder = app.MapGroup("api/users").WithTags("Users");

            userEndpointsBuilder.MapPost("register", async (UserRegisterDto userCredentials, IUserService service, CancellationToken cancellationToken) =>
            {
                await service.RegisterUser(userCredentials, cancellationToken);
                return Results.Ok();
            }).AddEndpointFilter<UserRegisterValidationFilter>().Produces(200).ProducesValidationProblem();

            userEndpointsBuilder.MapPost("login", async (UserLoginDto userCredentials, IUserService service, CancellationToken cancellationToken) =>
            {
                TokensBearerDto token = await service.LoginUser(userCredentials, cancellationToken);
                return Results.Ok(token);
            }).AddEndpointFilter<UserLoginValidationFilter>().Produces<TokensBearerDto>().ProducesValidationProblem();

            userEndpointsBuilder.MapPost("refresh", async (string refreshToken, IUserService service) =>
            {
                TokensBearerDto tokens = await service.RefreshTokens(refreshToken);

                return Results.Ok(tokens);
            }).Produces<TokensBearerDto>().ProducesValidationProblem().RequireAuthorization();
        }
    }
}