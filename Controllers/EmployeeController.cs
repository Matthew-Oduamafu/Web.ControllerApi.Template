﻿using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Web.ControllerApi.Template.Extensions;
using Web.ControllerApi.Template.Models;
using Web.ControllerApi.Template.Models.Dtos;
using Web.ControllerApi.Template.Services.Interfaces;

namespace Web.ControllerApi.Template.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
[ApiVersion("1.0")]
public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly ILinkService _linkService;


    public EmployeeController(IEmployeeService employeeService, LinkGenerator linkGenerator, ILinkService linkService)
    {
        _employeeService = employeeService;
        _linkService = linkService;
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
    [HttpGet("{id}", Name = nameof(Get))]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Get), OperationId = nameof(Get))]
    public async Task<IActionResult> Get(string id)
    {
        var apiResponse = await _employeeService.GetEmployeeByIdAsync(id);
        AddLinksForEmployee(apiResponse);
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
    [HttpGet("all", Name = nameof(GetAll))]
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
    /// Retrieves a paged list of employees based on the provided filter.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/employee/paged-list?Page=1&amp;PageSize=10
    ///
    /// Sample successful response:
    /// 
    ///     {
    ///         "Message": "Success",
    ///         "Code": 200,
    ///         "Data": {
    ///             "Page": 1,
    ///             "PageSize": 10,
    ///             "TotalPages": 5,
    ///             "TotalCount": 50,
    ///             "Items": [
    ///                 {
    ///                     "Id": "123e4567e89b12d3a456426614174001",
    ///                     "Name": "John Doe",
    ///                     "DOB": "1990-01-01T00:00:00",
    ///                     "JobTitle": "Software Engineer",
    ///                     "Salary": 75000,
    ///                     "HireDate": "2020-05-01T00:00:00",
    ///                     "CreatedAt": "2022-01-01T00:00:00",
    ///                     "Age": 33
    ///                 },
    ///                 // Additional employee items...
    ///             ]
    ///         },
    ///         "Errors": null
    ///     }
    ///
    /// Sample error response:
    ///
    ///     {
    ///         "Message": "Error occurred while fetching paged employee list",
    ///         "Code": 500,
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
    /// <param name="filter">The filter criteria for paging and filtering employee list.</param>
    /// <returns>An ApiResponse object containing the paged list of employee details.</returns>
    /// <response code="200">Returns the paged list of employees.</response>
    /// <response code="500">If an error occurs during the fetch operation.</response>
    [HttpGet("paged-list", Name = nameof(GetPageList))]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PagedList<EmployeeResponse>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(GetPageList), OperationId = nameof(GetPageList))]
    public async Task<IActionResult> GetPageList([FromQuery] BaseFilter filter)
    {
        var apiResponse = await _employeeService.GetEmployees(filter);
        AddLinksForPagedEmployee(apiResponse, filter);
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
    [HttpPost("create", Name = nameof(Create))]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Create), OperationId = nameof(Create))]
    public async Task<IActionResult> Create([FromBody] EmployeeRequest request)
    {
        var apiResponse = await _employeeService.AddEmployeeAsync(request);
        AddLinksForEmployee(apiResponse);
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
    [HttpPut("update/{id}", Name = nameof(Update))]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Update), OperationId = nameof(Update))]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] EmployeeRequest request)
    {
        var apiResponse = await _employeeService.UpdateEmployeeAsync(id, request);
        AddLinksForEmployee(apiResponse);
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
    [HttpDelete("delete/{id}", Name = nameof(Delete))]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(Delete), OperationId = nameof(Delete))]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var apiResponse = await _employeeService.DeleteEmployeeAsync(id);
        return ToActionResult(apiResponse);
    }

    private void AddLinksForEmployee(IApiResponse<EmployeeResponse> apiResponse)
    {
        apiResponse.Data?.Links.Add(
            _linkService.GenerateLink(nameof(Get), new { id = apiResponse.Data.Id }, "self", "GET"));
        apiResponse.Data?.Links.Add(
            _linkService.GenerateLink(nameof(Update), new { id = apiResponse.Data.Id }, "update-employee", "PUT"));
        apiResponse.Data?.Links.Add(
            _linkService.GenerateLink(nameof(Delete), new { id = apiResponse.Data.Id }, "delete-employee", "DELETE"));
    }

    private void AddLinksForPagedEmployee(IApiResponse<PagedList<EmployeeResponse>> apiResponse, BaseFilter filter)
    {
        if (apiResponse.Data?.Items == null || !apiResponse.Data.Items.Any()) return;

        apiResponse.Data.Links.Add(
            _linkService.GenerateLink(nameof(GetPageList),
                new { Page = filter.Page, PageSize = filter.PageSize }, "self", "GET"));
        
        if (apiResponse.Data.Page > 1)
        {
            apiResponse.Data.Links.Add(
                _linkService.GenerateLink(nameof(GetPageList),
                    new { Page = filter.Page - 1, PageSize = filter.PageSize }, "previous-page", "GET"));
        }

        if (apiResponse.Data.Page < apiResponse.Data.TotalPages)
        {
            apiResponse.Data.Links.Add(
                _linkService.GenerateLink(nameof(GetPageList),
                    new { Page = filter.Page + 1, PageSize = filter.PageSize }, "next-page", "GET"));
        }
    }
}