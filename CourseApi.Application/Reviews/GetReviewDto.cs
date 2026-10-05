using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Application.Reviews
{
    public class GetReviewDto
    {
        public int ReviewId { get; set; }

        public string ReviewText { get; set; } = null!;

        public double ReviewRating { get; set; }
    }
}