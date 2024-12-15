using Application;
using Application.Common.Helpers;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.FileProviders;
using MySql.EntityFrameworkCore.Extensions;
using WebUI.Filters;

const string allowSpecificOrigins = "AllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMySQLServer<FileManagerDbContext>(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
builder.Services.AddScoped<IFileManagerDbContext>(provider => provider.GetRequiredService<FileManagerDbContext>());
builder.Services.AddApplication();
builder.Services.AddSingleton<IPathService, PathService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

var app = builder.Build();

#region Create db
await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<FileManagerDbContext>();
await context.Database.EnsureCreatedAsync();
#endregion

app.UseHttpsRedirection();

var documentDirectory = Path.Combine(builder.Environment.WebRootPath,
    builder.Configuration.GetSection("FileManager:Path").Value ??
    throw new ArgumentNullException("Directory is not configured."));

DirectoryHelper.EnsureDirectoryExists(documentDirectory);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(documentDirectory), RequestPath = "/documents"
});
app.UseCors(allowSpecificOrigins);
app.MapControllers();
app.Run();



