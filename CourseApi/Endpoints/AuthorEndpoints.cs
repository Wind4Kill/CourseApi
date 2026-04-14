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
            var endpointBuilder = app.MapGroup("/authors").WithTags("Authors");

            endpointBuilder.MapPost("", async (IAuthorService service, CreateAuthorDto authorDto, LinkGenerator links) =>
            {
                  Author author = await service.CreateAuthor(authorDto);

                  string? link = links.GetPathByName("GetAuthorById", new { id = author.AuthorId });

                  return Results.Created(link, author);
            }).WithParameterValidation().Produces(201);

            endpointBuilder.MapGet("{id:int}", async (int id, IAuthorService service) =>
            {
                  GetAuthorByIdDto requestedAuthor = await service.GetAuthorById(id);
                  return Results.Ok(requestedAuthor);
            }).WithName("GetAuthorById").Produces(200);

            endpointBuilder.MapDelete("{id:int}", async (int id, IAuthorService service) =>
            {
                  int affectedRows = await service.DeleteAuthor(id);

                  return affectedRows is > 0 ? Results.NoContent() : Results.InternalServerError("Entity couldn't be updated");
            });

            endpointBuilder.MapPost("{id:int}", async (int id, IAuthorService service, CreateCourseDto createdCourseDto, LinkGenerator links) =>
            {
                  Course createdCourse = await service.AddCourseToAuthor(id, createdCourseDto);
                  string? link = links.GetPathByName("GetCourseById", new { Id = createdCourse.CourseId });

                  return Results.Created(link, createdCourse);
            });
      }
}
