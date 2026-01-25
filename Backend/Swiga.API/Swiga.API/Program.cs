using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swiga.Application.Services;
using Swiga.Domain;
using Swiga.Domain.Models;
using Swiga.Infrastructure;
using Swiga.Infrastructure.Repositories;
using Swiga.Domain.Abstructions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Swiga - service for rent inventory ",
        Version = "v1",
        Description = "",
        Contact = new OpenApiContact
        {
            Name = "Development Team",
            Email = "dev@swiga.com"
        }
    });

});

builder.Services.AddDbContext<SwigaDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("SwigaDbContext"));
    });

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IRentalPointService, RentalPointService>();
builder.Services.AddScoped<IRentalPointRepository, RentalPointRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", 
            "https://localhost:4200", 
            "http://127.0.0.1:51329", 
            "https://127.0.0.1:51329", 
            "https://localhost:7087",
            "http://localhost:51329",  
            "https://localhost:51329"    )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// После app = builder.Build();
app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
