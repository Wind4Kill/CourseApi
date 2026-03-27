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
           var builder= app.MapGroup("Course").WithTags("Courses");
            builder.MapPost("/All", async (ICourseService service, SortFilterOptions? options, LinkGenerator generator) =>
            {
                  if (options is null)
                  {
                        options = new SortFilterOptions();
                  }
                  List<GetCourseDto> courses = await service.GetCourses(options!);
                  return courses is not null ? Results.Ok(courses) :
                  Results.Problem(detail: "Courses haven't been found", statusCode: 404);
            }).Produces<Ok>().ProducesProblem(statusCode: 404);

            builder.MapPost("/", async (CreateCourseDto dto, ICourseService service, LinkGenerator generator) =>
            {
                  Course addedCourse = await service.CreateCourse(dto);

                  string? link = generator.GetPathByName("GetCourseById", new {id=addedCourse.CourseId});

            Results.Created(link, addedCourse);
            }).WithParameterValidation().Produces<Ok>().ProducesProblem(statusCode: 400);

            builder.MapGet("{id:int}", async (ICourseService service, int id) =>
            {
                  GetCourseByIdDto? requestedCourse = await service.GetCourseById(id);

                  return requestedCourse is null ?
                  Results.Problem(detail: "Requested course is not found", statusCode: 400)
                  : Results.Ok(requestedCourse);
            }).Produces<Ok>().ProducesProblem(statusCode: 400).WithName("GetCourseById");
            
      }


}
