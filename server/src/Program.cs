using Core;
using Core.Services.Model;
using Core.Services.Util; // Make sure this namespace is correct for RequestService
using server.Middleware; // Add this using directive for your custom middleware

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Transient: each instance will be used only one time, even in the same request
// Scoped: the same class in the same request
// Singleton: one instance shared between all request 
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<MongoDbService>();

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ProfileEventService>();
builder.Services.AddScoped<EventProfileService>();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await dbContext.Init();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger"; 
    });
}

// --- INSERT YOUR EXCEPTION HANDLING MIDDLEWARE HERE ---
// It should be placed early in the pipeline to catch exceptions
// from subsequent middleware components and your controllers.
// A good place is after routing but before authorization and endpoint mapping.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();