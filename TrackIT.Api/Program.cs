using TrackIT.Api.Dtos;
using TrackIT.Api.Data;
using TrackIT.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapCategoriesEndpoints();
app.MapTransactionsEndpoints();

app.Run();
