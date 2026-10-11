using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using FluentValidation;

namespace CourseApi.Api.Validators
{
    public class UserRegisterValidator:AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email can't be empty.")
            .EmailAddress().WithMessage("Input email address has an inappropriate format.");

            RuleFor(u => u.Password).NotEmpty().WithMessage("User password can't be empty.")
            .MinimumLength(8).WithMessage("Password must be minimum 8 characters in length");

            RuleFor(u => u.UserName).NotEmpty().WithMessage("Username can't be empty.")
            .MinimumLength(6).WithMessage("User name can't be less than 6 characters in length.")
            .MaximumLength(30).WithMessage("User name must be not greater than 30 characters in length.")
            .Must(username =>
            {
                return username.ToLower() != "admin";
            }).WithMessage("Provided username has an inappropriate value.");
        }
    }
}