using CatalogService.Application.Extensions;
using CatalogService.Infrastructure.Extensions;
using CatalogService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddGrpc();
var app = builder.Build();


app.MapGrpcService<CatalogGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");


app.Run();

