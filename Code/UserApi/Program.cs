using Authorization;
using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;

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

builder.Services.AddSingleton<IAuthorizationConfig, AuthorizationConfig>();
RegisterRepositoryDependencies(builder.Services);
RegisterServiceDependencies(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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