using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Enpoints;

public static class AuthorEndpoints
{
      public static void AddAuthorEndpoints(this WebApplication app)
      {
            var endpointBuilder = app.MapGroup("api/authors").WithTags("Authors");

            endpointBuilder.MapPost("", async (IAuthorService service, CreateAuthorDto authorDto,
             LinkGenerator links, CancellationToken cancellationToken) =>
            {
                  GetAuthorDto author = await service.CreateAuthor(authorDto, cancellationToken);

                  string? link = links.GetPathByName("GetAuthorById", new { id = author.AuthorId });

                  return Results.Created(link, author);
            }).WithParameterValidation().Produces<GetAuthorDto>(201).ProducesProblem(statusCode:400);

            endpointBuilder.MapGet("{id:int}", async (int id, IAuthorService service, CancellationToken cancellationToken) =>
            {
                  GetAuthorDto requestedAuthor = await service.GetAuthorById(id, cancellationToken);
                  return Results.Ok(requestedAuthor);
            }).WithName("GetAuthorById").Produces<GetAuthorDto>(200).ProducesProblem(statusCode:404);

            endpointBuilder.MapDelete("{id:int}", async (int id, IAuthorService service, CancellationToken cancellationToken) =>
            {
                  await service.DeleteAuthor(id, cancellationToken);

                  return Results.NoContent();
            }).Produces(204);

            endpointBuilder.MapPut("{id:int}", async (int id, IAuthorService service,
            CreateCourseDto createdCourseDto, LinkGenerator links, CancellationToken cancellationToken) =>
            {
                  GetCourseByIdDto createdCourse = await service.AddCourseToAuthor(id, createdCourseDto, cancellationToken);
                  string? link = links.GetPathByName("GetCourseById", new { Id = createdCourse.CourseId });

                  return Results.Created(link, createdCourse);
            }).Produces<GetCourseByIdDto>(204).ProducesProblem(statusCode:400);
      }
}
