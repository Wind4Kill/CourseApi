using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Domain.Entities;

namespace CourseApi.Data.Persistency.Repositories
{
    public class ReviewRepository(ApplicationContext applicationContext) : IReviewRepository
    {
        public Review AddReview(Review review)
        {
            applicationContext.Set<Review>().Add(review);
            return review;
        }
    }
}