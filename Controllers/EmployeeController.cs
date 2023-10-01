using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Web.ControllerApi.Template.Models;
using Web.ControllerApi.Template.Models.Dtos;
using Web.ControllerApi.Template.Services.Interfaces;

namespace Web.ControllerApi.Template.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
[ApiVersion("1.0")]
public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Fetches an employee's details by their unique ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/employee/123e4567e89b12d3a456426614174000
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Employee details fetched successfully",
    ///         "Code": 200,
    ///         "Data": {
    ///             "Id": "123e4567e89b12d3a456426614174000",
    ///             "Name": "John Doe",
    ///             "DOB": "1990-01-01T00:00:00",
    ///             "JobTitle": "Software Engineer",
    ///             "Salary": 75000,
    ///             "HireDate": "2020-05-01T00:00:00",
    ///             "CreatedAt": "2022-01-01T00:00:00",
    ///             "Age": 33
    ///         },
    ///         "Errors": null
    ///     }
    ///
    /// Sample error response:
    ///
    ///     {
    ///         "Message": "Error occurred while fetching employee details",
    ///         "Code": 400,
    ///         "Data": null,
    ///         "Errors": [
    ///             {
    ///                 "Field": "id",
    ///                 "ErrorMessage": "Invalid ID format"
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The unique identifier of the employee, represented as a GUID without hyphens.</param>
    /// <returns>The details of the specified employee wrapped in an ApiResponse object.</returns>
    /// <response code="200">Returns the employee details.</response>
    /// <response code="400">If the ID is not valid or any other error occurs.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Get), OperationId = nameof(Get))]
    public async Task<IActionResult> Get(string id)
    {
        var apiResponse = await _employeeService.GetEmployeeByIdAsync(id);
        return ToActionResult(apiResponse);
    }


    /// <summary>
    /// Fetches details of all employees.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/employee/all
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Success",
    ///         "Code": 200,
    ///         "Data": [
    ///             {
    ///                 "Id": "123e4567e89b12d3a456426614174001",
    ///                 "Name": "John Doe",
    ///                 "DOB": "1990-01-01T00:00:00",
    ///                 "JobTitle": "Software Engineer",
    ///                 "Salary": 75000,
    ///                 "HireDate": "2020-05-01T00:00:00",
    ///                 "CreatedAt": "2022-01-01T00:00:00",
    ///                 "Age": 33
    ///             }
    ///         ],
    ///         "Errors": null
    ///     }
    ///
    /// Sample error response:
    ///
    ///     {
    ///         "Message": "Error occurred while fetching employee details",
    ///         "Code": 400,
    ///         "Data": null,
    ///         "Errors": [
    ///             {
    ///                 "Field": "general",
    ///                 "ErrorMessage": "Internal server error"
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <returns>A list of employee details wrapped in an ApiResponse object.</returns>
    /// <response code="200">Returns the details of all employees.</response>
    /// <response code="400">If any error occurs during the fetch operation.</response>
    [HttpGet("all")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<EmployeeResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(GetAll), OperationId = nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var apiResponse = await _employeeService.GetEmployeesAsync();
        return ToActionResult(apiResponse);
    }

    /// <summary>
    /// Creates a new employee based on the provided details.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/employee/create
    ///
    ///     {
    ///         "Name": "John Doe",
    ///         "DOB": "1990-01-01T00:00:00",
    ///         "JobTitle": "Software Engineer",
    ///         "Salary": 75000,
    ///         "HireDate": "2020-05-01T00:00:00"
    ///     }
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Success",
    ///         "Code": 200,
    ///         "Data": {
    ///             "Id": "123e4567e89b12d3a456426614174001",
    ///             "Name": "John Doe",
    ///             "DOB": "1990-01-01T00:00:00",
    ///             "JobTitle": "Software Engineer",
    ///             "Salary": 75000,
    ///             "HireDate": "2020-05-01T00:00:00",
    ///             "CreatedAt": "2022-01-01T00:00:00",
    ///             "Age": 33
    ///         },
    ///         "Errors": null
    ///     }
    ///
    /// Sample error response:
    ///
    ///     {
    ///         "Message": "Error occurred while creating the employee",
    ///         "Code": 400,
    ///         "Data": null,
    ///         "Errors": [
    ///             {
    ///                 "Field": "Name",
    ///                 "ErrorMessage": "Employee name is required"
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="request">The employee details required to create a new employee record.</param>
    /// <returns>The details of the newly created employee wrapped in an ApiResponse object.</returns>
    /// <response code="200">Returns the details of the newly created employee.</response>
    /// <response code="400">If there are validation errors or any other issues during the creation process.</response>
    [HttpPost("create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Create), OperationId = nameof(Create))]
    public async Task<IActionResult> Create([FromBody] EmployeeRequest request)
    {
        var apiResponse = await _employeeService.AddEmployeeAsync(request);
        return ToActionResult(apiResponse);
    }


    /// <summary>
    /// Updates an existing employee's details based on the provided ID and request body.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/employee/update/123e4567e89b12d3a456426614174001
    ///
    ///     {
    ///         "Name": "John Doe Updated",
    ///         "DOB": "1990-01-01T00:00:00",
    ///         "JobTitle": "Senior Software Engineer",
    ///         "Salary": 85000,
    ///         "HireDate": "2020-05-01T00:00:00"
    ///     }
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Success",
    ///         "Code": 200,
    ///         "Data": {
    ///             "Id": "123e4567e89b12d3a456426614174001",
    ///             "Name": "John Doe Updated",
    ///             "DOB": "1990-01-01T00:00:00",
    ///             "JobTitle": "Senior Software Engineer",
    ///             "Salary": 85000,
    ///             "HireDate": "2020-05-01T00:00:00",
    ///             "CreatedAt": "2022-01-01T00:00:00",
    ///             "Age": 33
    ///         },
    ///         "Errors": null
    ///     }
    ///
    /// Sample error response:
    ///
    ///     {
    ///         "Message": "Error occurred while updating the employee",
    ///         "Code": 400,
    ///         "Data": null,
    ///         "Errors": [
    ///             {
    ///                 "Field": "Name",
    ///                 "ErrorMessage": "Employee name is required"
    ///             },
    ///             {
    ///                 "Field": "Id",
    ///                 "ErrorMessage": "Employee with the given ID does not exist"
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The unique ID of the employee to be updated.</param>
    /// <param name="request">The updated employee details.</param>
    /// <returns>The details of the updated employee wrapped in an ApiResponse object.</returns>
    /// <response code="200">Returns the details of the updated employee.</response>
    /// <response code="400">If there are validation errors, the employee doesn't exist, or any other issues during the update process.</response>
    [HttpPut("update/{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Update), OperationId = nameof(Update))]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] EmployeeRequest request)
    {
        var apiResponse = await _employeeService.UpdateEmployeeAsync(id, request);
        return ToActionResult(apiResponse);
    }


    /// <summary>
    /// Deletes an existing employee based on the provided ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/employee/delete/123e4567e89b12d3a456426614174001
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Success",
    ///         "Code": 200,
    ///         "Data": {
    ///             "Id": "123e4567e89b12d3a456426614174001",
    ///             "Name": "John Doe",
    ///             "DOB": "1990-01-01T00:00:00",
    ///             "JobTitle": "Software Engineer",
    ///             "Salary": 75000,
    ///             "HireDate": "2020-05-01T00:00:00",
    ///             "CreatedAt": "2022-01-01T00:00:00",
    ///             "Age": 33
    ///         },
    ///         "Errors": null
    ///     }
    ///
    /// Sample not found response:
    ///
    ///     {
    ///         "Message": "Employee with the given ID does not exist",
    ///         "Code": 404,
    ///         "Data": null,
    ///         "Errors": null
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The unique ID of the employee to be deleted.</param>
    /// <returns>An ApiResponse object indicating the result of the deletion.</returns>
    /// <response code="200">Returns the details of the deleted employee.</response>
    /// <response code="404">If the employee with the given ID does not exist.</response>
    [HttpDelete("delete/{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Delete), OperationId = nameof(Delete))]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var apiResponse = await _employeeService.DeleteEmployeeAsync(id);
        return ToActionResult(apiResponse);
    }
}