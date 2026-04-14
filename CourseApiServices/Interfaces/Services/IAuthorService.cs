using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CourseApiServices.Interfaces.Services;

public interface IAuthorService
{
      Task<Author> CreateAuthor(CreateAuthorDto authorDto);
      Task<GetAuthorByIdDto> GetAuthorById(int id);

      Task<int> DeleteAuthor(int id);

      Task<Course> AddCourseToAuthor(int authorId, CreateCourseDto courseDto);
            

}
