﻿using System.Linq.Expressions;
using EmployeesApi.Adapters;
using EmployeesApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeesApi.Domain;

public class MongDbEmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;
    private readonly FilterDefinition<Employee> _onlyActiveEmployees;
    private readonly ILookupSalary _salaryLookup;

    public MongDbEmployeeRepository(MongoDbContext context, ILookupSalary salaryLookup)
    {
        _context = context;
        _onlyActiveEmployees = Builders<Employee>.Filter.Where(emp => emp.InActive != true);
        _salaryLookup = salaryLookup;
    }


    public async Task<bool> ChangePropertyAsync<TField>(ObjectId id, Expression<Func<Employee, TField>> field, TField value)
    {
        var employeeFilter = Builders<Employee>.Filter.Where(emp => emp.Id == id);
        var filter = Builders<Employee>.Filter.And(_onlyActiveEmployees, employeeFilter);

        var update = Builders<Employee>.Update.Set(field, value);
        var result = await _context.GetEmployeeCollection().UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
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


        decimal salary = await _salaryLookup.GetSalaryForNewHireAsync(request.Department);

        var employeeToAdd = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Department = request.Department,
            Salary = salary
        };
        await _context.GetEmployeeCollection().InsertOneAsync(employeeToAdd);

        return new GetEmployeeDetailsResponse(employeeToAdd.Id.ToString(), employeeToAdd.FirstName, employeeToAdd.LastName, employeeToAdd.Phone, employeeToAdd.Email, employeeToAdd.Department);
    }
}
