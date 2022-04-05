using EmployeesApi.Models;
using MongoDB.Bson;

namespace EmployeesApi.Domain;

public interface IEmployeeRepository
{
    Task<GetEmployeeDetailsResponse?> GetEmployeeByIdAsync(ObjectId id);
}
