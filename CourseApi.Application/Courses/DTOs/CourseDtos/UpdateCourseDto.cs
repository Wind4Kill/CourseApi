using System.ComponentModel.DataAnnotations;


namespace CourseApi.Application.DTOs.CourseDtos;

public class UpdateCourseDto
{
      [MaxLength(50)]
      public string? CourseName { get; set; } = null!;

      public decimal? CoursePrice { get; set; }

      [MaxLength(250)]
      public string? CourseDescription { get; set; } = null!;

      public string? Author{ get; set; }
      public List<string>? Categories { get; set; } = null!;


}
