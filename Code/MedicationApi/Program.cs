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
builder.Services.AddLinqToDBContext<AppDataConnection>((provider, options) => options
                .UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                .UseDefaultLogging(provider));

RegisterRepositoryDependencies(builder.Services);

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
    services.AddScoped<IMedicationRepository, MedicationRepository>();
}