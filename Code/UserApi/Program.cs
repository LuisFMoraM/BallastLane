using Authorization;
using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
            "<li>A user may be created in the system</li>" +
            "<li>A created user can log in to the system</li>" +
            "<li>A logged-in user can perform several operations with Medications. " +
                "<a href='http://localhost:5096/swagger/index.html' target='_blank'>See Medications Endpoints</a></li>" +
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

builder.Services.AddSingleton<IAuthorizationConfig, AuthorizationConfig>();
RegisterRepositoryDependencies(builder.Services);
RegisterServiceDependencies(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrescriberPoint v1"));
}

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
    services.AddScoped<IUserRepository, UserRepository>();
}

/// <summary>
/// Add service dependencies to the Container
/// </summary>
/// <param name="services">Container instance</param>
void RegisterServiceDependencies(IServiceCollection services)
{
    services.AddScoped<IUserService, UserService>();
}