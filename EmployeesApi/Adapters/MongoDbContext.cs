using MongoDB.Driver;

namespace EmployeesApi.Adapters;

public class MongoDbContext
{
    private readonly IMongoCollection<Employee> _employeesCollection;
    public MongoDbContext()
    {
        var clientSettings = MongoClientSettings.FromConnectionString("mongodb://admin:TokyoJoe138!@localhost:27017/");

        var conn = new MongoClient(clientSettings);

        var db = conn.GetDatabase("employees_db"); // TODO: Don't do this. Use configuration. Coming Later.

        _employeesCollection = db.GetCollection<Employee>("employees");
    }

    public IMongoCollection<Employee> GetEmployeeCollection() => _employeesCollection;

}


