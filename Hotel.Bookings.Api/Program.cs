using HotelBookings.Application;
using HotelBookings.Application.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var config = builder.Configuration;

builder.Services.AddDbContext<HotelBookings.Application.Context.ApplicationContext>(options => options.UseSqlServer(config.GetConnectionString("ConnStr")));

builder.Services.AddSwaggerGen();

builder.Services.AddContosoServices();

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