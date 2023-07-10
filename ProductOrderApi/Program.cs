using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Data;
using ProductOrderApi.Data.Repositories;
using ProductOrderApi.Helpers;
using ProductOrderApi.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    })
//    .AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//})
    ;
//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//    );

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseInMemoryDatabase("OrderDB"));
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<DataHelper>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ProductOrderApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductOrderApi v1"));

app.MapControllers();

app.Run();


public partial class Program { }