using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Review AddReview(Review review);
    }
}