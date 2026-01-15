using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Implementations;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Implementations;
using TekCandor.Repository.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<ICycleRepository, CycleRepository>();
builder.Services.AddScoped<ICycleService, CycleService>();
builder.Services.AddScoped<IHubRepository, HubRepository>();
builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IReturnReasonRepository, ReturnReasonRepository>();
builder.Services.AddScoped<IReturnReasonService, ReturnReasonService>();



// Swagger & Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();


