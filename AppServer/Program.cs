using BL;
using DAL;
using DAL.Entities;
using System.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using DAL.Models;
using IronPython.Runtime;
using IronPython.Hosting;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*
//Script Python
// Create an instance of the Python runtime
var engine = Python.CreateEngine();

// Set the search path for the Python script
var searchPaths = engine.GetSearchPaths();
searchPaths.Add("./Emulator");
engine.SetSearchPaths(searchPaths);

// Import the Python module
dynamic datasimulator = engine.ImportModule("datasimulator");*/
//Script Python

var app = builder.Build();

//test
/*
var dbContext = new DBContext();
var dal = new DAL.DAL(dbContext);
app.Use(async (context, next) =>
{
    context.Items["IDAL"] = dal;
    await next.Invoke();
});
*/
//tests

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//MAIN
var bl = new BL.BL();
//await bl.testtest();
//await bl.StoreGamesAsync("Skyrim");
List<string> gameNames= new List<string>() { "Rocket League","Elden Ring","Goat Simulator","Cuphead","Hollow Knight","Hogwarts Legacy","God of War","Sekiro","Worms W.M.D","Counter Strike Global offensive","Hades","The binding of isaac","PUBG"};
foreach (string gameName in gameNames)await bl.StoreGameAsync(gameName);
//List<PlayersTime> playersTimelist = await bl.RetrieveNumberOfPlayersTime("Dota 2", new DateTime(2023, 4, 18, 0, 0, 0), new DateTime(2023, 4, 19, 1, 0, 0));
//foreach (var player in playersTimelist) Console.WriteLine(player+"\n");
//await bl.StoreGameAsync("Dota 2");
List<string> serverNames = new List<string>() { "PUBG", "Elden Ring", "Hogwarts Legacy", "Worms W.M.D", "Counter Strike Global offensive", "The binding of isaac" };
foreach (string serverName in serverNames) await bl.StoreServerAsync(serverName);
await bl.StoreServerAsync("PUBG");

/*
 * var gameData = await bl.RetrieveServerFromApiAsync("Rocket League");
if (gameData != null)
{
    Console.WriteLine($"Game data for 'Pokemon': {JsonSerializer.Serialize(gameData)}");
}
else
{
    Console.WriteLine("Error retrieving game data from Python API.");
}
*/

app.Run();
