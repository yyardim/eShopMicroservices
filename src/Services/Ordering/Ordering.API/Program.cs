using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// **************************************
// Add services to the container
// **************************************
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


WebApplication app = builder.Build();


// **************************************
// Configure the HTTP request pipeline
// **************************************

app.Run();
