using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;

namespace CourseApi.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void AddUserEndpoints(this WebApplication app)
        {
            var userEndpointsBuilder = app.MapGroup("api/users").WithTags("Users");

            userEndpointsBuilder.MapPost("register", async (UserRegisterDto userCredentials) =>
            {
                
            });
        }
    }
}