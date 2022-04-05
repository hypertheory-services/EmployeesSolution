using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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

public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; } = "";
    [BsonElement("lastName")]
    public string LastName { get; set; } = "";
    [BsonElement("email")]
    public string Email { get; set; } = "";
    [BsonElement("phone")]
    public string Phone { get; set; } = "";
    [BsonElement("department")]
    public string Department { get; set; } = "";
    [BsonElement("salary")]
    public decimal Salary { get; set; }
}

/*
 * {
    _id: ObjectId('624c5858eee281eac4e3fb76'),
    firstName: 'Bob',
    lastName: 'Smith',
    email: 'bob@aol.com',
    phone: '555-1212',
    department: 'DEV',
    salary: 120000
}*/
