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
                  if(options is null)
                  {
                        options = new SortFilterOptions();
                  }
                  List<GetCourseDto> courses = await service.GetCourses(options!);
                  return courses is not null ? Results.Ok(courses) :
                  Results.Problem(detail: "Courses haven't been found", statusCode: 404);
            }).Produces<Ok>().ProducesProblem(statusCode: 404);

            builder.MapPost("/Create", async (CreateCourseDto dto, ICourseService service) =>
            {
                  int affectedRows = await service.CreateCourse(dto);

                  string link = "";

                  return affectedRows is not 0 ? Results.Created() : Results.Problem(detail: "Course hasn't been added", statusCode: 400);
            }).WithParameterValidation().Produces<Ok>().ProducesProblem(statusCode:400);
      }


}
