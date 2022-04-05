using EmployeesApi.Models;

namespace EmployeesApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse> GetEmployeeByIdAsync(string id);
}
