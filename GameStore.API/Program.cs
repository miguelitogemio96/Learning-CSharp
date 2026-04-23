using GameStore.API.Data;
using GameStore.API.Routes;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();

app.MigrateDB();
app.MapGameEndpoints();
app.MapGenresEndpoints();

app.Run();
