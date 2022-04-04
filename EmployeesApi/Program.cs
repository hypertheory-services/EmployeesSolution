// This compiles to the Program class with a static Main method.
// It is the entry point for our API. When we start the API, it starts here. 
// When this is done running, the application quits.
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Above here is configuring "behind the scenes stuff"
var app = builder.Build();
// Below here is configuring the Http "Pipeline" - how requests and responses are made.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.Run(); // this is our Kestrel Web Server running. 
