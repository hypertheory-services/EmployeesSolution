// This compiles to the Program class with a static Main method.
// It is the entry point for our API. When we start the API, it starts here. 
// When this is done running, the application quits.

// Prior to .NET 5, Web APIs used an open source library called NewtonSoft.Json
// In .NET 5 + they use their own. This System.Test.Json
using System.Text.Json.Serialization;
using EmployeesApi;
using EmployeesApi.Adapters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("bsonid", typeof(BsonIdConstraint));
});
// Configuration Stuff

builder.Services.Configure<MongoConnectionOptions>(builder.Configuration.GetSection(MongoConnectionOptions.SectionName));

// CORS for out of cluster browsers etc.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(pol =>
    {
        pol.AllowAnyOrigin();
        pol.AllowAnyHeader();
        pol.AllowAnyMethod();
    });
});
// IOptions<MongoConnectionOptions>

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // No brainer. Always do this.
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // This is optional but I like it.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // OpenAPI Specification. "Swagger Documents"

// Domain Services
builder.Services.AddScoped<IEmployeeRepository, MongoDbEmployeeRepository>();
builder.Services.AddScoped<ILookupSalary, RpcSalaryLookup>();

// Adapter Services
builder.Services.AddSingleton<MongoDbContext>(); // Created "lazily"
//var mongoDbContext = new MongoDbContext();
//// configure the thing, etc.
//builder.Services.AddSingleton(mongoDbContext);

// Typed Client
builder.Services.AddHttpClient<SalaryApiContext>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("salaryApiUrl"));
}).AddPolicyHandler(InClusterPolicies.GetRetryPolicy());


// Above here is configuring "behind the scenes stuff"
var app = builder.Build();
// Below here is configuring the Http "Pipeline" - how requests and responses are made.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.Run(); // this is our Kestrel Web Server running. 
