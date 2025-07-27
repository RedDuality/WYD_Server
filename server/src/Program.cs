using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Configures Swagger document generation

// Register CosmosDbContext as a singleton
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

// Initialize MongoDbContext after the app is built
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await dbContext.Init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development environment
    app.UseSwagger(); // Serves the generated Swagger JSON document
    app.UseSwaggerUI(options =>
    {
        // Specify the Swagger JSON endpoint.
        // By default, AddSwaggerGen generates the document at /swagger/v1/swagger.json
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger"; // Sets the Swagger UI at the root /swagger
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
