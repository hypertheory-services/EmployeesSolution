using EmployeesApi.Adapters;
using EmployeesApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeesApi.Domain;

public class MongDbEmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;
    private readonly FilterDefinition<Employee> _onlyActiveEmployees;
    public MongDbEmployeeRepository(MongoDbContext context)
    {
        _context = context;
        _onlyActiveEmployees = Builders<Employee>.Filter.Where(emp => emp.InActive != true);
    }

    public async Task FireAsync(ObjectId objectId)
    {
        var filter = Builders<Employee>.Filter.Where(emp => emp.Id == objectId);
        var update = Builders<Employee>.Update.Set(emp => emp.InActive, true);
        await _context.GetEmployeeCollection().UpdateOneAsync(filter, update);
    }

    public async Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id)
    {
        // In the context we have employees, but we need a GetEmployeeDetailResponse
        // Moving data from one thing into another "mapping", "projecting"
        var projection = Builders<Employee>.Projection.Expression(emp =>
        new GetEmployeeDetailsResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Phone, emp.Email, emp.Department)
        );
        //var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        var filterByThisEmployee = Builders<Employee>.Filter.Where(emp => emp.Id == id);
        var filter = Builders<Employee>.Filter.And(_onlyActiveEmployees, filterByThisEmployee);
        var response = await _context.GetEmployeeCollection().Find(filter)
            .Project(projection)
            .SingleOrDefaultAsync(); // I am expecting 1 or 0 things to be returned here.

        return response;

    }

    public async Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync()
    {
        var projection = Builders<Employee>.Projection.Expression(emp => new GetEmployeeSummaryResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Department));

        var employees = await _context.GetEmployeeCollection().Find(_onlyActiveEmployees) // Give them all to me!
                .Project(projection)
                .ToListAsync();
        return new GetCollectionResponse<GetEmployeeSummaryResponse>() { Data = employees };
    }

    public async Task<GetEmployeeDetailsResponse> HireEmployee(PostEmployeeRequest request)
    {
        var employeeToAdd = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Department = request.Department,
            Salary = 100000
        };
        await _context.GetEmployeeCollection().InsertOneAsync(employeeToAdd);

        return new GetEmployeeDetailsResponse(employeeToAdd.Id.ToString(), employeeToAdd.FirstName, employeeToAdd.LastName, employeeToAdd.Phone, employeeToAdd.Email, employeeToAdd.Department);
    }
}
