using EmployeesApi.Adapters;
using EmployeesApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeesApi.Domain;

public class MongDbEmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;

    public MongDbEmployeeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<GetEmployeeDetailsResponse> GetEmployeeByIdAsync(ObjectId id)
    {
        // In the context we have employees, but we need a GetEmployeeDetailResponse
        // Moving data from one thing into another "mapping", "projecting"
        var projection = Builders<Employee>.Projection.Expression(emp =>
        new GetEmployeeDetailsResponse(emp.Id.ToString(), emp.FirstName, emp.LastName, emp.Phone, emp.Email, emp.Department)
        );
        //var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        var response = await _context.GetEmployeeCollection().Find(options => options.Id == id)
            .Project(projection)
            .SingleOrDefaultAsync(); // Todo:

        return response;

    }
}
