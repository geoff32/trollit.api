using Serilog;
using TrollIt.Api.Account.DependencyInjection;
using TrollIt.Api.Exceptions;
using TrollIt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Filter.With<ManagedExceptionLogEventFilter>()
    .WriteTo.Console());

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
