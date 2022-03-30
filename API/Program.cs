using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Product), typeof(ProductToReturnDto));
builder.Services.AddDbContext<StoreContext>((optionsBuilder) =>
optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplicationServices();
builder.Services.AddCors(opt => 
{
    opt.AddPolicy("CorsPolicy", policy => 
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});


var app = builder.Build();
using(var scope = app.Services.CreateScope()) {//run database migration when app runs.
    var services = scope.ServiceProvider;
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
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
