using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using CourseApi.Api.HelpClasses;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourseApi;

public static class ProgramExtensions
{

      public static async Task MigratePendingMigrations(this WebApplication app)
      {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                  var strategy = context.Database.CreateExecutionStrategy();

                  await strategy.ExecuteAsync(async () =>
                  {
                        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                        if (pendingMigrations.Any())
                        {
                              await context.Database.MigrateAsync();
                        }
                  });
            }
      }
      public static async Task SeedData(this WebApplication app)
      {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                  if (!context.Courses.Any())
                  {
                        context.Courses.AddRange(
                             new Course()
                             {
                                   CourseName = "C#",
                                   CourseDetails = new CourseDetails
                                   {
                                         CourseDescription = "Advanced C#",
                                         CoursePrice = 1000
                                   },
                                   Author = new Author() { Name = "Andrew Troelsen" },
                                   Categories = new List<Category>() { new Category { Name = "C#" } }
                             }
                       );

                        await context.SaveChangesAsync();
                  }
            }
      }

      public static async Task AddAdmin(this WebApplication app)
      {
            await using var scope = app.Services.CreateAsyncScope();

            UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            DbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                  try

                  {
                        var transaction = await dbContext.Database.BeginTransactionAsync();
                        User admin = new(app.Configuration["AdminCredentials:UserName"]!)
                        {
                              Email = app.Configuration["AdminCredentials:Email"],
                        };

                        await userManager.CreateAsync(admin, app.Configuration["AdminCredentials:Password"]!);

                        Claim role = new Claim("Role", "Admin");

                        await userManager.AddClaimAsync(admin, role);

                        await transaction.CommitAsync();
                  }
                  catch (Exception)
                  {
                        throw;
                  }

            });

      }

      public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
      {
            // services.AddHsts(configureOptions =>
            // {
            //       configureOptions.MaxAge = TimeSpan.FromMinutes(60);
            // });

            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHttpContextAccessor();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
            services.ConfigureHttpJsonOptions(options =>
            {
                  options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddAuthentication(options =>
            {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                  options.TokenValidationParameters = new()
                  {
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                  };
            });

            services.AddAuthorization(options =>
            {
                  options.AddPolicy("IsAdmin", policy => policy.RequireClaim("UserName", configuration["AdminCredentials:UserName"]!).RequireClaim("Role", "Admin"));
            });

            if (environment.IsDevelopment())
            {
                  services.AddDistributedMemoryCache();
            }

            if (environment.IsProduction())
            {
                  services.AddStackExchangeRedisCache(options =>
                  {
                        options.Configuration = configuration.GetConnectionString("RedisConnection");
                        options.InstanceName = "CourseApi_cache";
                  });

                  services.AddStackExchangeRedisOutputCache(options =>
                  {
                        options.Configuration = configuration.GetConnectionString("RedisConnection");
                        options.InstanceName = "CourseApi_cache";
                  });
            }

            services.AddOutputCache();
            services.AddProblemDetails();

            //remove IsProduction in production
            if (environment.IsDevelopment() || environment.IsProduction())
            {
                  services.AddEndpointsApiExplorer();
                  services.AddSwaggerGen();
                  services.AddHealthChecks();
            }

            return services;

      }
}
