using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreSphere.IdentityAccess.Infrastructure.Persistence;
using StoreSphere.IdentityAccess.Infrastructure.Extensions;
using StoreSphere.IdentityAccess.Application;
using StoreShpere.SharedKernel.Extensions;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// Database: EF Core for Identity + Domain
// --------------------------------------------------------------------
// --------------------------------------------------------------------
// Shared DbContext registration (single database, dbo schema)
// --------------------------------------------------------------------
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityAccessConnection")));

builder.Services.AddDbContext<DomainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityAccessConnection")));

// --------------------------------------------------------------------
// Identity (ASP.NET Core Identity + EF store)
// --------------------------------------------------------------------
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
// --------------------------------------------------------------------
// JWT Settings
// --------------------------------------------------------------------
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// --------------------------------------------------------------------
// Application + Infrastructure + Shared Kernel
// --------------------------------------------------------------------
builder.Services.AddIdentityAccessInfrastructure();
builder.Services.AddSharedKernelEvents();
builder.Services.AddApplicationServices();
// Controllers, Swagger, MediatR
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --------------------------------------------------------------------
// Middleware pipeline
// --------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
