using System.Reflection;
using Application;
using Application.Common.Interfaces;
using Infrastructure.Context;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMySQLServer<FileManagerDbContext>(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
builder.Services.AddScoped<IFileManagerDbContext>(provider => provider.GetRequiredService<FileManagerDbContext>());
builder.Services.AddApplication();

var app = builder.Build();

#region Create db
await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<FileManagerDbContext>();
await context.Database.EnsureCreatedAsync();
#endregion

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();



