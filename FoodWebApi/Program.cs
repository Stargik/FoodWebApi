
using FoodMVCWebApp;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Services;
using FoodWebApi.Interfaces;
using FoodWebApi.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FoodWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<FoodDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.FoodDbConnection))
        );

        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));
        builder.Services.AddTransient<IImageService, FilesystemImageService>();
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString(SettingStrings.RedisConnection);
            options.InstanceName = builder.Configuration["Redis:InstanceName"];
        });
        builder.Services.AddTransient<ICacheService, RedisCacheService>();

        builder.Services.AddDishElasticsearch(builder.Configuration);

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

        app.MigrateDatabase();

        app.Run();
    }
}

