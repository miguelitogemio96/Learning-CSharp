using GameStore.API.Data;
using GameStore.API.Routes;

var connString = "Data Source=GameStore.db";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MigrateDB();
app.MapGameEndpoints();

app.Run();
