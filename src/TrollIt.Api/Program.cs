using System.Data.Common;
using TrollIt.Api.Account.DependencyInjection;
using TrollIt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWebApiAuthentication();

builder.Services.AddDomain();
builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("postgres") ?? throw new ArgumentException("No connection found");
builder.Services.AddInfrastructure(new InfrastructureOptions(connectionString));

var app = builder.Build();

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
