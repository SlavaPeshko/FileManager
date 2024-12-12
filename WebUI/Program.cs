using Data.Context;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMySQLServer<FileManagerContext>(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);

var app = builder.Build();

#region Create db
await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<FileManagerContext>();
await context.Database.EnsureCreatedAsync();
#endregion

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();



