#pragma warning disable CS8618

namespace Web.ControllerApi.Template.Models.Dtos;

public class EmployeeRequest
{
    public string Name { get; set; }
    public DateTime DOB { get; set; }
    public string JobTitle { get; set; }
    public double Salary { get; set; }
}

public class EmployeeResponse : EmployeeRequest
{
    public string Id { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Age => DateTime.Now.Year - DOB.Year;
}