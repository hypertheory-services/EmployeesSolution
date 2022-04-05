using EmployeesApi.Models;

namespace EmployeesApi.Domain;

public class MongDbEmployeeRepository : IEmployeeRepository
{
    public async Task<GetEmployeeDetailsResponse> GetEmployeeByIdAsync(string id)
    {

        var response = new GetEmployeeDetailsResponse(id, "Joe", "Schmidt", "888-1212", "joe@aol.com", "Sales");
        return response;


    }
}
