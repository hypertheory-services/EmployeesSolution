using EmployeesApi.Models;
namespace EmployeesApi.Controllers;


public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("employees/{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        GetEmployeeDetailsResponse response = await _employeeRepository.GetEmployeeByIdAsync(id);
        return Ok(response);

    }

}
