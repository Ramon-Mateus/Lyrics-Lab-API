using Lyrics_Lab.Contexts;
using Lyrics_Lab.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Repositories;
using Lyrics_Lab.Services;
using Lyrics_Lab.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=todos.sqlite3"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ISongService, SongService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigins", policy =>
  {
    policy.WithOrigins("http://localhost:3000") // Replace with your allowed origins
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Needed if you're using cookies or authorization headers
  });
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddControllers();

builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = "JwtBearer";
  options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", configureOptions =>
{
  configureOptions.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
    {
      var jwtService = builder.Services.BuildServiceProvider().GetService<JwtService>();
      return new[] { jwtService.GetSymmetricSecurityKey() };
    },
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
  };

  configureOptions.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      context.Token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      return Task.CompletedTask;
    }
  };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
