using catalog.Core.Locating.Application.Persistence.QueryObject;
using catalog.Core.Locating.Domain.Location;
using catalog.Core.SharedKernel.Base.Specification;
using catalog.Persistence.DBLocating.Context;
using Microsoft.EntityFrameworkCore;

namespace catalog.Persistence.DBLocating.QueryObject;

/// <summary>
/// QueryObject для получения данных об объектах местоположений.
/// Реализация через EF Core, БД Locating.
/// </summary>
public class LocationObjectQueryObject : ILocationObjectQueryObject
{
    private readonly DbContextOptions<LocatingContext> options;

    public LocationObjectQueryObject(DbContextOptions<LocatingContext> options)
    {
        this.options = options;
    }

    public async Task<List<LocationObject>> GetAsync(Specification<LocationObject> specification)
    {
        using var context = new LocatingContext(options);
        return await context.LocationObjects.Where(specification.ToExpression()).ToListAsync();
    }
}
