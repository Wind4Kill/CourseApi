using CourseApi.Application.DTOs.AuthorDtos;
using CourseApi.Application.DTOs.CourseDtos;


namespace CourseApi.Application.Interfaces.Services;

public interface IAuthorService
{
      Task<GetAuthorDto> CreateAuthor(CreateAuthorDto authorDto, CancellationToken cancellationToken);
      Task<GetAuthorDto> GetAuthorById(int id, CancellationToken cancellationToken);

      Task DeleteAuthor(int id, CancellationToken cancellationToken);

      Task<GetCourseByIdDto> AddCourseToAuthor(int authorId, CreateCourseDto courseDto, CancellationToken cancellationToken);
            

}
