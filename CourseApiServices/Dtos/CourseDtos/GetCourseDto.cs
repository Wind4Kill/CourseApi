using System;
using AutoMapper;
using CourseApiDomain.Entities;

namespace CourseApiServices.Dtos.CourseDtos;

[AutoMap(typeof(Course))]
public class GetCourseDto
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public double? CourseRating { get; set; }

      public decimal CoursePrice { get; set; }

}
