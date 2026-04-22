using GameStore.API.DTOs;
using GameStore.API.Routes;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGameEndpoints();

app.Run();
