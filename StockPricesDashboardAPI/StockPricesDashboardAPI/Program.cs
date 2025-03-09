using MongoDB.Driver;
using StackExchange.Redis;
using StockPricesDashboardAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StockPricesDashboardAPI.Domain.Model;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AlphaVantageConfig>(builder.Configuration.GetSection("AlphaVantage"));

var redisConnectionString = builder.Configuration["RedisSettings:ConnectionString"];
ConfigurationOptions con = new ConfigurationOptions()
{
    SyncTimeout = 500000,
    EndPoints =
    {
        redisConnectionString 
    },
    AbortOnConnectFail = false
};
var redis = ConnectionMultiplexer.Connect(con);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddScoped<RedisService>();


builder.Services.AddHttpClient<StockPriceService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["AuthSettings:Authority"];
        options.Audience = builder.Configuration["AuthSettings:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Allow requests from this origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
