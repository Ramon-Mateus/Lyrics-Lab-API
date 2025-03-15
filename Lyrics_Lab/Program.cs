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
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

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
    policy.WithOrigins(Environment.GetEnvironmentVariable("CORS_ORIGIN")!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
  });
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

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
