var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. adým Microsoft.Extensions.Caching.StackExchangeRedis paketinin yüklenmesi
// 2. adým service entegration (redis sunucumuz hangi port a karþýlýk geliyorsa baðlantý kurmak için onu yazýyoruz)
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
