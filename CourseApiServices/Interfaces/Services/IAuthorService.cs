using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;

namespace CourseApiServices.Interfaces.Services;

public interface IAuthorService
{
      Task<Author> CreateAuthor(CreateAuthorDto authorDto);
      Task<GetAuthorByIdDto> GetAuthorById(int id);
}
