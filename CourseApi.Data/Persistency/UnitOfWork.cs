using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Data.Persistency.Repositories;

namespace CourseApi.Data.Persistency
{
    public class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}