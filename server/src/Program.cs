using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString =
        Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
        ?? builder.Configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});

// Optional: Add a service for your specific database/collection
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var databaseName =
        Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
        ?? builder.Configuration.GetValue<string>("MongoDB:DatabaseName");
    return client.GetDatabase(databaseName ?? "wyd");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
