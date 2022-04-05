using EmployeesApi.Models;
using MongoDB.Bson;

namespace EmployeesApi.Controllers;


public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("employees/{id:bsonid}")]
    public async Task<ActionResult> GetById(ObjectId id)
    {

        GetEmployeeDetailsResponse response = await _employeeRepository.GetEmployeeByIdAsync(id);
        return Ok(response);

    }

}
