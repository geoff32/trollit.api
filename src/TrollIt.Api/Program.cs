using Serilog;
using TrollIt.Api.Account.DependencyInjection;
using TrollIt.Api.Exceptions;
using TrollIt.Infrastructure;
using TrollIt.Infrastructure.Mountyhall;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Filter.With<ManagedExceptionLogEventFilter>()
    .WriteTo.Console());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWebApiAuthentication();

builder.Services.AddDomain();
builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("postgres") ?? throw new InvalidOperationException("No connection found");
var mountyhallOptions = builder.Configuration.GetRequiredSection("Mountyhall").Get<MountyhallOptions>()
    ?? throw new InvalidOperationException("Mountyhall settings not found");
builder.Services.AddInfrastructure(new InfrastructureOptions(connectionString, mountyhallOptions));

builder.Services.AddPortableObjectLocalization();

builder.Services
    .Configure<RequestLocalizationOptions>(options => options
        .AddSupportedCultures("fr")
        .AddSupportedUICultures("fr"));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManagedExceptionErrorHandler>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }