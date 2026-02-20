
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Implementations;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Implementations;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<ICycleRepository, CycleRepository>();
builder.Services.AddScoped<ICycleService, CycleService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IHubRepository, HubRepository>();
builder.Services.AddScoped<IHubService, HubService>();

builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();

builder.Services.AddScoped<IReturnReasonRepository, ReturnReasonRepository>();
builder.Services.AddScoped<IReturnReasonService, ReturnReasonService>();
builder.Services.AddScoped<ITokenRevocationRepository, TokenRevocationRepository>();

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IImportHistoryRepository, ImportHistoryRepository>();
builder.Services.AddScoped<IImportHistoryService, ImportHistoryService>();
builder.Services.AddScoped<IChequeDepositImportService, ChequeDepositImportService>();

builder.Services.AddScoped<IImportDataRepository, ImportDataRepository>();
builder.Services.AddScoped<IManualImportDataRepository, ManualImportDataRepository>();
builder.Services.AddScoped<IChequeDepositRepository, ChequeDepositRepository>();
// Swagger & Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serialize enums as strings in JSON
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TekCandor API", Version = "v1" });
    // Show TimeSpan as string in Swagger with an example
    options.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("09:00:00"),
        Description = "Time in HH:mm:ss"
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter only the JWT token. The 'Bearer ' prefix is added automatically.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        //policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");


// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty)),
        ClockSkew = TimeSpan.FromSeconds(5)
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(jti))
            {
                context.Fail("Missing jti");
                return;
            }

            var repo = context.HttpContext.RequestServices
                .GetRequiredService<ITokenRevocationRepository>();

            if (await repo.IsRevokedAsync(jti))
            {
                context.Fail("Token revoked");
            }
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();


