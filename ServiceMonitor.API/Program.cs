using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.API.Extensions;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Application.Extensions;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Infrastructure.Extensions;
using ServiceMonitor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

var app = builder.Build();

app.MapControllers();

    
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapGroup("api/identity").MapIdentityApi<User>();

app.UseMiddleware<ErrorHandlingMiddleware>();


app.Run();