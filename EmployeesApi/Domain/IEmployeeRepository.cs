using System.Linq.Expressions;
using EmployeesApi.Models;
using MongoDB.Bson;

namespace EmployeesApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id);
    Task<GetCollectionResponse<GetEmployeeSummaryResponse>> GetEmployeesAsync();
    Task<GetEmployeeDetailsResponse> HireEmployee(PostEmployeeRequest request);
    Task FireAsync(ObjectId objectId);
    Task<bool> ChangePropertyAsync<TField>(ObjectId id, Expression<Func<Employee, TField>> field, TField value);
}
