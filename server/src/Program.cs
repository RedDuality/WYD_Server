using Core.Services.Model;
using Core.Services.Util; 
using Core.Services.Database; 
using Core.Services.Interfaces;
using server.Middleware;
using server.External;
using Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PublicApiPolicy", builder =>
    {
        builder
            .AllowAnyOrigin() // Public API: allow all origins
            .AllowAnyHeader() // Allow headers like Authorization
            .AllowAnyMethod(); // GET, POST, PUT, DELETE, etc.
    });
});

//default route /api/
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefixConvention("api"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Transient: each instance will be used only one time, even in the same request
// Scoped: the same class in the same request
// Singleton: one instance shared between all request 
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<MongoDbService>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddSingleton<IAuthenticationService, FirebaseAuthService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ProfileDetailsService>();

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

app.UseCors("PublicApiPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();