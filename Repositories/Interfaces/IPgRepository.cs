namespace Web.ControllerApi.Template.Repositories.Interfaces;

public interface IPgRepository
{
    Task<int> SaveChangesAsync();
}