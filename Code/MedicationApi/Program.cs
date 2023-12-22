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
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PrescriberPoint -> Proof of Concept (PoC)",
        Version = "v1",
        Description = "The main goals of the PoC are centered around the concepts that:<br>" +
        " <ul>" +
            "<li>A Medication list is exposed to be consumed by anyone</li>" +
            "<li>A Medication can be reviewed by anyone searching by its Id</li>" +
            "<li>A registered/logged-in user can:</li>" +
                "<ul>" +
                    "<li>Create a Medication</li>" +
                    "<li>Update a Medication</li>" +
                    "<li>Delete a Medication</li>" +
                "</ul>" +
        "</ul>"
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrescriberPoint v1"));
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