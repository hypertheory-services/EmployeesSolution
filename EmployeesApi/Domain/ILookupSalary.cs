namespace EmployeesApi.Domain;

public interface ILookupSalary
{
    Task<decimal> GetSalaryForNewHireAsync(string department);
}
