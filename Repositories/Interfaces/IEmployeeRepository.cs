using Web.ControllerApi.Template.Data.Entities;

namespace Web.ControllerApi.Template.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(string id);
    Task<IReadOnlyList<Employee>> GetEmployeesAsync();
    Task<Employee> AddEmployeeAsync(Employee employee);
    Employee UpdateEmployeeAsync(Employee employee);
    Employee DeleteEmployeeAsync(Employee employee);
}