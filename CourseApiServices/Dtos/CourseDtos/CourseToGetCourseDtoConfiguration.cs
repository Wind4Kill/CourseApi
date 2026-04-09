using System;
using AutoMapper;
using CourseApiDomain;
using CourseApiDomain.Entities;

namespace CourseApiServices.Dtos.CourseDtos;

public class CourseToGetCourseDtoConfiguration:Profile
{
      public CourseToGetCourseDtoConfiguration()
      {
            CreateMap<Course, GetCourseDto>()
            .ForMember(c => c.CoursePrice,
            map => map.MapFrom(c => c.CourseDetails.CoursePrice))
            .ForMember(c => c.CourseRating,
            map => map.MapFrom(c => ApplicationContext.GetCourseRating(c.CourseId)));
      }
}
