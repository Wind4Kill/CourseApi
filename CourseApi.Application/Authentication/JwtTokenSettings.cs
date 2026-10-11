using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Application.Authentication
{
    public class JwtTokenSettings
    {
        public double Expiration { get; set; }
        public string SecretKey { get; set; } = null!;
    }
}