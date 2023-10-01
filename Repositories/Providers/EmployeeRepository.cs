using Microsoft.EntityFrameworkCore;
using Web.ControllerApi.Template.Data;
using Web.ControllerApi.Template.Data.Entities;
using Web.ControllerApi.Template.Repositories.Interfaces;

namespace Web.ControllerApi.Template.Repositories.Providers;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(string id)
    {
        return await _dbContext.Employees.FindAsync(id);
    }

    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync()
    {
        return await _dbContext.Employees.AsTracking().ToListAsync();
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        await _dbContext.Employees.AddAsync(employee);
        return employee;
    }

    public Employee UpdateEmployeeAsync(Employee employee)
    {
        _dbContext.Employees.Update(employee);
        return employee;
    }

    public Employee DeleteEmployeeAsync(Employee employee)
    {
        _dbContext.Employees.Remove(employee);
        return employee;
    }
}