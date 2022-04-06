using EmployeesApi.Adapters;

namespace EmployeesApi.Domain;

public class RpcSalaryLookup : ILookupSalary
{

    private readonly SalaryApiContext _context;

    public RpcSalaryLookup(SalaryApiContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetSalaryForNewHireAsync(string department)
    {
        var request = new SalaryRequest {  department = department };

        var response = await _context.GetSalary(request); // addd an issue for the new hire to make this async. ;)

        return response.amount;
    }
}
