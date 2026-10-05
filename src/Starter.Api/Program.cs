using Starter.Api.Common.Authentication;
using Starter.Api.Common.Configuration;
using Starter.Api.Common.Exceptions;
using Starter.Api.Common.Extensions;
using Starter.Api.ReportingEntities.Create;
using Starter.Api.ReportingEntities.GetById;
using Starter.Api.ReportingPeriods.Create;
using Starter.Application;
using Starter.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddStarterOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<RouteHandlerOptions>(options =>
{
    options.ThrowOnBadRequest = false;
});

builder.Services.AddApplication(builder.Configuration);
builder.AddInfrastructure();

builder.Services.AddStarterAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddStarterAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseStarterOpenApi();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapCreateReportingPeriod();
app.MapCreateReportingEntity();
app.MapGetReportingEntity();

await app.RunAsync();
