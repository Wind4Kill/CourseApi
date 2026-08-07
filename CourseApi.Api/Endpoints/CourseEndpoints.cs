using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseApi.Enpoints;

public static class CourseEndpoints
{
      public static void AddCourseEndpoints(this WebApplication app)
      {
            var endpointBuilder = app.MapGroup("api/courses").WithTags("Courses");

            endpointBuilder.MapPost("", async ([FromBody] CreateCourseDto dto, ICourseService service, LinkGenerator links) =>
           {
                 Course course = await service.CreateCourse(dto);
                 string? link = links.GetPathByName("GetCourseById", new { id = course.CourseId });
                 return Results.Created(link, course);

           }).WithParameterValidation().Produces(201);

            endpointBuilder.MapGet("", async (ICourseService service, [AsParameters] Filtering options) =>
            {
                  SortFilterOptions sortFilterOptions = new();
                  if (Enum.TryParse<SortingOptions>(options.Sorting!, true, out SortingOptions sortingOptions))
                  {
                        sortFilterOptions.Sorting = sortingOptions;
                  }
                  if (Enum.TryParse<FilterOptions>(options.Filter, true, out FilterOptions filterOptions))
                  {
                        sortFilterOptions.Filter = filterOptions;
                  }
                  if (!string.IsNullOrEmpty(options.FilterValue))
                  {
                        sortFilterOptions.FilterValue = options.FilterValue;
                  }
                  if (options.PageNum is not null && options.PageNum.HasValue)
                  {
                        sortFilterOptions.PageNum = options.PageNum.Value;
                  }

                  List<GetCourseDto> courses = await service.GetCourses(sortFilterOptions!);
                  return Results.Ok(courses);

            }).Produces(200).CacheOutput(builder => builder.Expire(TimeSpan.FromSeconds(120)).Tag("all-books"));

            endpointBuilder.MapGet("{id:int}", async (ICourseService service, int id) =>
            {
                  GetCourseByIdDto? requestedCourse = await service.GetCourseById(id);
                  return Results.Ok(requestedCourse);
            }).Produces(200).WithName("GetCourseById");

            endpointBuilder.MapPatch("{id:int}", async (int id, UpdateCourseDto updatedCourse, ICourseService service, IOutputCacheStore store) =>
                       {
                             await service.UpdateCourse(id, updatedCourse);
                             await store.EvictByTagAsync("all-books",default);

                             return Results.NoContent();

                       }).WithParameterValidation().Produces(204);

            endpointBuilder.MapDelete("{id:int}", async (int id, ICourseService service, IOutputCacheStore store) =>
            {
                  int affectedRows = await service.RemoveCourse(id);
                  await store.EvictByTagAsync("all-books", default);

                  return Results.NoContent();

            }).Produces(204);

      }

}
