using TrackIT.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.MapCategoriesEndpoints();

app.MapTransactionsEndpoints();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();
