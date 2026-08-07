using System;
using CourseApi.Api.FiltrationClasses;
using CourseApi.Domain.HelpClasses;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseApi.Enpoints;

public static class CourseEndpoints
{
      public static void AddCourseEndpoints(this WebApplication app)
      {
            var endpointBuilder = app.MapGroup("api/courses").WithTags("Courses");

            endpointBuilder.MapPost("", async ([FromBody] CreateCourseDto dto,
             ICourseService service, LinkGenerator links, CancellationToken cancellationToken) =>
           {
                 GetCourseByIdDto course = await service.CreateCourse(dto, cancellationToken);
                 string? link = links.GetPathByName("GetCourseById", new { id = course.CourseId });
                 return Results.Created(link, course);

           }).WithParameterValidation().Produces(201);

            endpointBuilder.MapGet("", async (ICourseService service, [AsParameters] Filtering options, CancellationToken cancellationToken) =>
            {
                  SortFilterOptions sortFilterOptions = new(sortingOptions: options.Sorting, filterOptions: options.Filter,
                   filterValue: options.FilterValue, page: options.PageNum);

                  List<GetCourseDto> courses = await service.GetCourses(sortFilterOptions!, cancellationToken);
                  return Results.Ok(courses);

            }).AddEndpointFilter(new FiltrationFilter()).
            Produces(200).CacheOutput(builder => builder.Expire(TimeSpan.FromSeconds(120)).Tag("all-books"));

            endpointBuilder.MapGet("{id:int}", async (ICourseService service, int id, CancellationToken cancellationToken) =>
            {
                  GetCourseByIdDto? requestedCourse = await service.GetCourseById(id, cancellationToken);
                  return Results.Ok(requestedCourse);
            }).Produces(200).WithName("GetCourseById");

            endpointBuilder.MapPatch("{id:int}", async (int id, UpdateCourseDto updatedCourse,
            ICourseService service, IOutputCacheStore store, CancellationToken cancellationToken) =>
                       {
                             await service.UpdateCourse(id, updatedCourse, cancellationToken);
                             await store.EvictByTagAsync("all-books",default);

                             return Results.NoContent();

                       }).WithParameterValidation().Produces(204);

            endpointBuilder.MapDelete("{id:int}", async (int id, ICourseService service,
             IOutputCacheStore store, CancellationToken cancellationToken) =>
            {
                  await service.RemoveCourse(id, cancellationToken);
                  await store.EvictByTagAsync("all-books", default);

                  return Results.NoContent();

            }).Produces(204);

      }

}
