using Lyrics_Lab.Contexts;
using Lyrics_Lab.Data;
using Lyrics_Lab.Helpers;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options
    .WithOrigins(new[] {"http://localhost:3000", "http://localhost:8080", "http://localhost:4200"}) // Colocar a(s) porta(s) que o front-end vai utilizar para permitir acesso à API
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
