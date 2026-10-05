using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApiServices.Dtos.CourseDtos;
using FluentValidation;

namespace CourseApi.Api.Validators
{
    public class CreateCourseValidator:AbstractValidator<CreateCourseDto>
    {
        public CreateCourseValidator()
        {
            RuleFor(c => c.CourseName).NotEmpty().WithMessage("Course title can't be empty.")
            .MaximumLength(50).WithMessage("Course title length can't exceed 50 symbols.");

            RuleFor(c => c.CourseDescription).NotEmpty().WithMessage("Course description can't be empty.").
            MaximumLength(250).WithMessage("Course description length can't exceed 250 symbols.");

            RuleFor(c => c.CoursePrice).NotEmpty().WithMessage("Course price field can't be empty.").
            GreaterThan(0).WithMessage("Course price must be greater than 0.");

            RuleFor(c => c.Author).NotEmpty().WithMessage("Author name field can't be empty.");

            RuleFor(c => c.Categories).NotEmpty().WithMessage("Course must have at least one category.");
        
        }
    }
}