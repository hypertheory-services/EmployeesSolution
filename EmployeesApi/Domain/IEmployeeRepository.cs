using EmployeesApi.Models;
using MongoDB.Bson;

namespace EmployeesApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id);
    Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync();
    Task<GetEmployeeDetailsResponse> HireEmployee(PostEmployeeRequest request);
}
