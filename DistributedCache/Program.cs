var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. ad�m Microsoft.Extensions.Caching.StackExchangeRedis paketinin y�klenmesi
// 2. ad�m service entegration (redis sunucumuz hangi port a kar��l�k geliyorsa ba�lant� kurmak i�in onu yaz�yoruz)
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:1453");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
