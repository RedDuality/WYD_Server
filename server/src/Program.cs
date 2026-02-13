using Core.Model.Users;

using Core.Services.Users;
using Core.Services.Profiles;
using Core.Services.Masks;
using Core.Services.Events;
using Core.Services.Util;
using Core.Services.Notifications;
using Core.Services.Communities;

using Core.Components.Database;
using Core.Components.MessageQueue;
using Core.Components.ObjectStorage;
using Core.Components.ServerSentMessages;

using Core.External.Authentication;
using Core.External.Interfaces;
using Core.External.FCM;

using server.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;


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


builder.Services.AddControllers();

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

builder.Services.AddAuthorization(options =>
{
    foreach (UserClaimType claimType in Enum.GetValues<UserClaimType>())
    {
        string policyName = claimType.ToString();
        options.AddPolicy(policyName, policy =>
            policy.Requirements.Add(new UserClaimRequirement(claimType)));
    }
});

// for ContextManager
builder.Services.AddHttpContextAccessor();



// Transient: each instance will be re-created every time it is called, even in the same request
// Scoped: the same class in the same request
// Singleton: one instance shared between all request 
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<MongoDbService>();
builder.Services.AddSingleton<ISseService, SseService>();

builder.Services.AddScoped<IAuthService, FirebaseAuthService>();

builder.Services.AddSingleton<IAuthorizationHandler, UserAuthorizationService>();
builder.Services.AddSingleton<FCMService>();

builder.Services.AddSingleton<MinioClient>();

builder.Services.AddSingleton<IMessageQueueService, MessageQueueService>();
builder.Services.AddScoped<MessageQueueHandlerService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<UserClaimService>();

builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<WebConnectionService>();

builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ProfileDetailsService>();
builder.Services.AddScoped<ProfileTagService>();
builder.Services.AddScoped<ProfileProfileService>();
builder.Services.AddScoped<ProfileUpdatePropagationService>();

builder.Services.AddScoped<ImportedProfilesService>();

builder.Services.AddScoped<MaskService>();
builder.Services.AddScoped<MaskProfileService>();
builder.Services.AddScoped<EventMaskService>();

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<EventUpdatePropagationService>();

builder.Services.AddScoped<EventDetailsService>();
builder.Services.AddScoped<ProfileEventService>();
builder.Services.AddScoped<EventProfileService>();

builder.Services.AddScoped<MediaService>();

builder.Services.AddScoped<CommunityService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<ProfileCommunityService>();
builder.Services.AddScoped<CommunityProfileService>();

builder.Services.AddScoped<IContextManager, ContextManager>();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<BroadcastService>();
builder.Services.AddScoped<ProfileIdResolverFactory>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Wyd API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var minioClient = scope.ServiceProvider.GetRequiredService<MinioClient>();
    await dbContext.Init();
    await minioClient.TestConnection();
}

if (app.Environment.IsEnvironment("Local"))
{
    var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>().Value;
    mvcOptions.Conventions.Add(new RoutePrefixConvention("wyd/api"));

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("PublicApiPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();