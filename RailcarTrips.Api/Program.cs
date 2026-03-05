using Microsoft.EntityFrameworkCore;

using RailcarTrips.Application.Contracts;
using RailcarTrips.Application.Services;
using RailcarTrips.Application.Services.RailcarTrips.Application.Services;
using RailcarTrips.Core.Contracts;
using RailcarTrips.Core.Engine;
using RailcarTrips.Infrastructure;
using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Converters;
using RailcarTrips.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDevClient", policy =>
    {
        policy.WithOrigins("https://localhost:7029") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("RailcarTripsDb"));

builder.Services.AddScoped<ITripProcessor, TripProcessor>();
builder.Services.AddScoped<ITimeZoneConverter, TimeZoneConverter>(); 
builder.Services.AddScoped<ITripQueryService, TripQueryService>();
builder.Services.AddScoped<ITripImportService, TripImportService>();
builder.Services.AddScoped<ICitiesImportService, CitiesImportService>(); 
builder.Services.AddScoped<ICitiesRepository, EfCitiesRepository>();
builder.Services.AddScoped<ITripRepository, EfTripRepository>(); 


var app = builder.Build();

app.UseCors("AllowBlazorDevClient");

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
