using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Application.Authentication
{
    public class JwtTokenSettings
    {
        public DateTime Expiration { get; set; }
        public string SecretKey { get; set; } = null!;
    }
}