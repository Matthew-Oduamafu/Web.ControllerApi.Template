using Web.ControllerApi.Template.Models;
using Web.ControllerApi.Template.Models.Dtos;

namespace Web.ControllerApi.Template.Services.Interfaces;

public interface IEmployeeService
{
    Task<IApiResponse<EmployeeResponse>> GetEmployeeByIdAsync(string id);
    Task<IApiResponse<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync();
    Task<IApiResponse<EmployeeResponse>> AddEmployeeAsync(EmployeeRequest employeeRequest);
    Task<IApiResponse<EmployeeResponse>> UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest);
    Task<IApiResponse<EmployeeResponse>> DeleteEmployeeAsync(string id);
}