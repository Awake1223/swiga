using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swiga.Infrastructure;


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

var app = builder.Build();

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
