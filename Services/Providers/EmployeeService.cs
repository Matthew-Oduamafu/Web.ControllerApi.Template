using Mapster;
using Web.ControllerApi.Template.Data.Entities;
using Web.ControllerApi.Template.Extensions;
using Web.ControllerApi.Template.Models;
using Web.ControllerApi.Template.Models.Dtos;
using Web.ControllerApi.Template.Repositories.Interfaces;
using Web.ControllerApi.Template.Services.Interfaces;

namespace Web.ControllerApi.Template.Services.Providers;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IPgRepository _pgRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IPgRepository pgRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _pgRepository = pgRepository;
        _logger = logger;
    }

    public async Task<IApiResponse<EmployeeResponse>> GetEmployeeByIdAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return ApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employee by id {Id}", id);
            return ApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IApiResponse<IReadOnlyList<EmployeeResponse>>> GetEmployeesAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (employees.Count == 0)
                return ApiResponse<IReadOnlyList<EmployeeResponse>>.Default.ToNotFoundApiResponse();

            return employees.Adapt<IReadOnlyList<EmployeeResponse>>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employees");
            return ApiResponse<IReadOnlyList<EmployeeResponse>>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IApiResponse<PagedList<EmployeeResponse>>> GetEmployees(BaseFilter filter)
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsQueryable()
                .ToPagedList(filter.Page, filter.PageSize);

            return employees.Adapt<PagedList<EmployeeResponse>>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employees");
            return ApiResponse<PagedList<EmployeeResponse>>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IApiResponse<EmployeeResponse>> AddEmployeeAsync(EmployeeRequest employeeRequest)
    {
        try
        {
            var employee = employeeRequest.Adapt<Employee>();
            await _employeeRepository.AddEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding employee");
            return ApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IApiResponse<EmployeeResponse>> UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return ApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            employee = employeeRequest.Adapt(employee);
            _employeeRepository.UpdateEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employee");
            return ApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }

    public async Task<IApiResponse<EmployeeResponse>> DeleteEmployeeAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return ApiResponse<EmployeeResponse>.Default.ToNotFoundApiResponse();

            _employeeRepository.DeleteEmployeeAsync(employee);
            await _pgRepository.SaveChangesAsync();
            return employee.Adapt<EmployeeResponse>().ToOkApiResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting employee");
            return ApiResponse<EmployeeResponse>.Default.ToInternalServerErrorApiResponse();
        }
    }
}