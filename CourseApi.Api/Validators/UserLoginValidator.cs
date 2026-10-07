using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication.DTOs;
using FluentValidation;

namespace CourseApi.Api.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email can't be empty.")
               .EmailAddress().WithMessage("Input email address has an inappropriate format.");

            RuleFor(u => u.Password).NotEmpty().WithMessage("User password can't be empty.")
            .MinimumLength(8).WithMessage("Password must be minimum 8 characters in length");
        }
    }
}