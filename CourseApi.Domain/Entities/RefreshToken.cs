using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }

        public User User { get; set; } = null!;

        public string UserId { get; set; } = null!;
    }
}