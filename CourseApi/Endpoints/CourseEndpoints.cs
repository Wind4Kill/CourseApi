using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CourseApi.Enpoints;

public static class CourseEndpoints
{
      public static void AddCourseEndpoints(this WebApplication app)
      {
            var endpointBuilder = app.MapGroup("/courses").WithTags("Courses");

            endpointBuilder.MapPost("", async (CreateCourseDto dto, ICourseService service, LinkGenerator links) =>
           {
                 Course course = await service.CreateCourse(dto);
                 string? link = links.GetPathByName("GetCourseById", new { id = course.CourseId });
                 return Results.Created(link, course);

           }).WithParameterValidation().Produces(201);

            endpointBuilder.MapGet("", async (ICourseService service, [AsParameters] Filtering options) =>
            {
                  SortFilterOptions sortFilterOptions = new()
                  {
                        Sorting = (SortingOptions)Enum.Parse(typeof(SortingOptions), options.Sorting!),
                        Filter = (FilterOptions)Enum.Parse(typeof(FilterOptions), options.Filter!),
                        FilterValue = options.FilterValue,
                        PageNum = options.PageNum!.Value
                  };

                  List<GetCourseDto> courses = await service.GetCourses(sortFilterOptions!);

                  return Results.Ok(courses);

            }).Produces(200);

            endpointBuilder.MapGet("{id:int}", async (ICourseService service, int id) =>
            {
                  GetCourseByIdDto? requestedCourse = await service.GetCourseById(id);
                  return requestedCourse is null ? Results.NotFound() : Results.Ok(requestedCourse);
            }).Produces(200).WithName("GetCourseById");

            endpointBuilder.MapPatch("{id:int}", async (int id, UpdateCourseDto updatedCourse, ICourseService service) =>
                       {

                             int affectedRows = await service.UpdateCourse(id, updatedCourse);

                             return affectedRows is > 0 ? Results.NoContent() :
                             Results.Problem(detail: "Entity couldn't be updated", statusCode: 400);



                       }).WithParameterValidation().Produces(204).ProducesProblem(400);

            endpointBuilder.MapDelete("{id:int}", async (int id, ICourseService service) =>
            {
                  int affectedRows = await service.RemoveCourse(id);

                  return affectedRows is > 0 ? Results.NoContent() :
                   Results.InternalServerError("Removal wasn't successfull");

            }).Produces(204).ProducesProblem(500);

      }

}
