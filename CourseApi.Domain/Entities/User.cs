using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CourseApi.Domain.Entities
{
    public class User:IdentityUser
    {
        public User(string name) : base(userName: name) { }

        public User() { }
        
        
    }
}