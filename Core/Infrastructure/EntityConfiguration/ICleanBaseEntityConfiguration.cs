using Core.Application.ServiceLifeTimes;
using Core.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EntityConfiguration;

public interface ICleanBaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>, ICleanBaseIgnore
    where TEntity : CleanEntity
{
}
