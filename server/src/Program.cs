using Core.Services.Model;
using Core.Services.Util;
using Core.Components.Database;
using Core.External.Interfaces;
using Core.External.Authentication;
using server.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PublicApiPolicy", builder =>
    {
        builder
            .AllowAnyOrigin() // Public API: allow all origins(CORS)
            .AllowAnyHeader() // Allow headers like Authorization
            .AllowAnyMethod(); // GET, POST, PUT, DELETE, etc.
    });
});

//default route /wyd/api/
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefixConvention("wyd/api"));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.Authority = builder.Configuration["AUTHENTICATION_ISSUER_URL"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["AUTHENTICATION_AUDIENCE"],
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(context.Exception.ToString());
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
//for ContextManager
builder.Services.AddHttpContextAccessor();



// Transient: each instance will be used only one time, even in the same request
// Scoped: the same class in the same request
// Singleton: one instance shared between all request 
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<MongoDbService>();

builder.Services.AddScoped<ContextManager>();
builder.Services.AddScoped<ContextService>();

string authProvider = builder.Configuration["AUTHENTICATION_PROVIDER"]!;
switch (authProvider)
{
    case "Firebase":
        builder.Services.AddSingleton<IAuthenticationService, FirebaseAuthService>();
        break;
    default:
        throw new Exception("Your authentication provider is not currently supported");
}


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ProfileDetailsService>();

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<ProfileEventService>();
builder.Services.AddScoped<EventProfileService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Wyd API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await dbContext.Init();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PublicApiPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();