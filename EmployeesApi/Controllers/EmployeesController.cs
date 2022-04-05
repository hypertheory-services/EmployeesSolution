using EmployeesApi.Models;
using MongoDB.Bson;

namespace EmployeesApi.Controllers;


[Route("employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ActionResult> AddEmployee([FromBody] PostEmployeeRequest request)
    {



        // 2. Save the thing to the database...
        GetEmployeeDetailsResponse response = await _employeeRepository.HireEmployee(request);

        // 3. Return
        //    - 201 Created Status Code
        //    - Include a header in the response with the Location of the new employee 
        //      Location: http://localhost:1337/employees/3893893898398 
        //    - Just send them a copy of whatever they would get if they went to that location.

        return CreatedAtRoute("employees#getbyid", new { id = response.id }, response);
    }
    // GET /employees
    [HttpGet("")]
    public async Task<ActionResult> GetAllEmployees()
    {
        GetCollectionResponse<GetEmployeeSummaryResponse> response = await _employeeRepository.GetEmployeesAsync();
        return Ok(response);
    }

    // GET /employees/:id
    [HttpGet("{id:bsonid}", Name ="employees#getbyid")] // it won't even create this controller if that id isn't a valid bsonid (return 404)
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
