using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace Web.ControllerApi.Template.Models.Dtos;

public class EmployeeRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Employee name is required")]
    public string Name { get; set; }
    public DateTime Dob { get; set; }
    public string JobTitle { get; set; }
    public double Salary { get; set; }
}

public class EmployeeResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Dob { get; set; }
    public string JobTitle { get; set; }
    public double Salary { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Age => DateTime.Now.Year - Dob.Year;

    public List<Link> Links { get; set; } = new();
}