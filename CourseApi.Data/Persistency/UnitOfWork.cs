using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Persistency
{
    public class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Course)
                    {
                        var databaseValues = await entry.GetDatabaseValuesAsync();

                        if (databaseValues is null)
                        {
                            throw new InvalidOperationException("Entity has been deleted by another user.");
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException("Concurrency conflict can't be resolved." + entry.Metadata.Name);
                    }
                }
            }
        }
    }
}