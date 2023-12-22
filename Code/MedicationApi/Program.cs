using Authorization;
using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddLinqToDBContext<AppDataConnection>((provider, options) => options
                .UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                .UseDefaultLogging(provider));

var securityKey = AuthorizationConstants.SecurityKey;
var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AuthorizationConstants.SecurityTokenIssuer,
            ValidAudience = AuthorizationConstants.SecurityTokenAudience,
            IssuerSigningKey = symmetricSecurityKey
        };
    });

RegisterRepositoryDependencies(builder.Services);
RegisterServiceDependencies(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Add repository dependencies to the Container
/// </summary>
/// <param name="services">Container instance</param>
void RegisterRepositoryDependencies(IServiceCollection services)
{
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IMedicationRepository, MedicationRepository>();
}

/// <summary>
/// Add service dependencies to the Container
/// </summary>
/// <param name="services">Container instance</param>
void RegisterServiceDependencies(IServiceCollection services)
{
    services.AddScoped<IMedicationService, MedicationService>();
}