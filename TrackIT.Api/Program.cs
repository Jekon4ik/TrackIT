using TrackIT.Api.Data;
using TrackIT.Api.Endpoints;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("TrackIT");
builder.Services.AddSqlite<TrackITContext>(connString);

var app = builder.Build();


app.MapCategoriesEndpoints();

app.MapTransactionsEndpoints();

app.MigrateDb();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();


