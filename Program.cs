
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapCarter();

app.Run();