using Web.ControllerApi.Template.Data;
using Web.ControllerApi.Template.Repositories.Interfaces;

namespace Web.ControllerApi.Template.Repositories.Providers;

public class PgRepository : IPgRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PgRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}