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

    [HttpGet("employees/{id:bsonid}")] // it won't even create this controller if that id isn't a valid bsonid (return 404)
    public async Task<ActionResult<GetEmployeeDetailsResponse>> GetById(string id)
    {
        var objectId = ObjectId.Parse(id); // try catch.
        GetEmployeeDetailsResponse? response = await _employeeRepository.GetEmployeeByIdAsync(objectId);
        if(response == null)
        {
            return NotFound();
        } else
        {
            return Ok(response);
        }

    }

}
