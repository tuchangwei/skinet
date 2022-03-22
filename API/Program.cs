using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Product), typeof(ProductToReturnDto));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddDbContext<StoreContext>((optionsBuilder) =>
optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
using(var scrope = app.Services.CreateScope()) {//run database migration when app runs.
    var services = scrope.ServiceProvider;
    var loggerFoctory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();

        await StoreContextSeed.SeedAsync(context, loggerFoctory);
    }
    catch (Exception ex)
    {
       var logger = loggerFoctory.CreateLogger<Program>();
       logger.LogError(ex, "An error occurred during migration");

    }

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
