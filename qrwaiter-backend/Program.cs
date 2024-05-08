using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using qrwaiter_backend.Data;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Mapping;
using qrwaiter_backend.Repositories;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Services;
using qrwaiter_backend.Services.Interfaces;
using System;
using System.Configuration;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using qrwaiter_backend.Middlewares;


public class Program
{
    public static void Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);

        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        builder.WebHost.UseUrls(builder.Configuration.GetSection("Urls").Value ?? "");
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DatabaseConnection")));




        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

        builder.Services.AddScoped<ITableService, TableService>();
        builder.Services.AddScoped<ITableRepository, TableRepository>();

        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        
        builder.Services.AddScoped<IQrCodeService, QrCodeService>();
        builder.Services.AddScoped<IQrCodeRepository, QrCodeRepository>();

        builder.Services.AddScoped<IStatisticQrCodeRepository, StatisticQrCodeRepository>();

        
        builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build()
            );



        //builder.Services.AddAutoMapper()
        builder.Services.AddAutoMapper(typeof(MappingProfiles));



        builder.Services.AddIdentityApiEndpoints<qrwaiter_backend.Data.Models.ApplicationUser>()
                        //.AddRoles<IdentityRole>()
                        //.AddClaimsPrincipalFactory<AppClaimsFactory>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();
        // Add services to the container.


        ;
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
                ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value ?? "")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,

            };
        });
        builder.Services.AddAuthorization();
        //.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        //builder.Services.AddAuthorization();


        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
            });
        }); 
        builder.Services.AddApplicationInsightsTelemetry();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("https://localhost:4200").AllowAnyHeader()
                                    .WithMethods("PUT", "DELETE", "GET", "POST");
                    
                });
        });
        builder.Services.AddControllers();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapGroup("/api/account").MapIdentityApi<qrwaiter_backend.Data.Models.ApplicationUser>();

        app.UseMiddleware<ResponseErrorHandlerMiddleware>().UseResponseCaching();


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();


        app.MapControllers();

        app.Run();

    }
}