using BL;
using DAL;
using System.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using DAL.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dbContext = new DBContext();
var dal = new DAL.DAL(dbContext);
app.Use(async (context, next) =>
{
    context.Items["IDAL"] = dal;
    await next.Invoke();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var bl = new BL.BL();
await bl.StoreGamesAsync("Pokemon");

app.Run();
