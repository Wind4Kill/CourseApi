using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CourseApi.Enpoints;

public static class CourseEndpoints
{
      public static void AddCourseEndpoints(this WebApplication app)
      {
            var builder = app.MapGroup("Course").WithTags("Courses");
            builder.MapPost("/All", async (ICourseService service, SortFilterOptions? options, LinkGenerator generator) =>
            {
                  if (options is null)
                  {
                        options = new SortFilterOptions();
                  }
                  List<GetCourseDto> courses = await service.GetCourses(options!);
                  return courses is not null ? Results.Ok(courses) :
                  Results.Problem(detail: "Courses haven't been found", statusCode: 404);
            }).Produces(200).ProducesProblem(statusCode: 404);

            builder.MapPost("", async (CreateCourseDto dto, ICourseService service, LinkGenerator generator) =>
            {
                  (int affectedRows, string addedCourse) = await service.CreateCourse(dto);

                  string? link = generator.GetPathByName("GetCourseById", new { id = dto.CourseId });

                  return affectedRows is > 0 ? Results.Created(link, addedCourse) : Results.Problem(detail: "Course hasn't been added", statusCode: 400);
            }).WithParameterValidation().Produces(201).ProducesProblem(statusCode: 400);

            builder.MapGet("{id:int}", async (ICourseService service, int id) =>
            {
                  GetCourseByIdDto? requestedCourse = await service.GetCourseById(id);

                  return requestedCourse is null ?
                  Results.Problem(detail: "Requested course is not found", statusCode: 400)
                  : Results.Ok(requestedCourse);
            }).Produces(204).ProducesProblem(statusCode: 400).WithName("GetCourseById");

            builder.MapDelete("{id:int}", async (int id, ICourseService service) =>
            {
                  try
                  {
                        int affectedRows = await service.RemoveCourse(id);

                        return affectedRows is > 0 ? Results.NoContent() :
                         Results.Problem(detail: "Removal wasn't successfull", statusCode: 404);

                  }
                  catch (ArgumentNullException ex)
                  {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
                  }

            }).Produces<Ok>().ProducesProblem(404).ProducesProblem(500);

            builder.MapPatch("", async (UpdateCourseDto updatedCourse, ICourseService service) =>
            {
                  try
                  {
                        int affectedRows = await service.UpdateCourse(updatedCourse);

                        return affectedRows is > 0 ? Results.NoContent() :
                        Results.Problem(detail: "Entity couldn't be updated", statusCode: 400);

                  }
                  catch (ArgumentNullException ex)
                  {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
                  }

            }).WithParameterValidation().Produces(204).ProducesProblem(400);


      }


}
