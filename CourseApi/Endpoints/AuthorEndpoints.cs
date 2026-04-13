using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
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
            
      
      }
}
