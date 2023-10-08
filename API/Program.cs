using System.Reflection;
using API.Extension;
using Application.Repository;
using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAplicacionServices();
builder.Services.AddDbContext<GeneratorPDFDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("ConexMysql");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	try
	{
		var context = services.GetRequiredService<GeneratorPDFDbContext>();
		await context.Database.MigrateAsync();
		await GeneratorPDFDbContextSeed.SeedAsync(context,loggerFactory);
	}
	catch (Exception ex)
	{
		var _logger = loggerFactory.CreateLogger<Program>();
		_logger.LogError(ex, "Ocurrio un error durante la migracion");
	}
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
