using System.Text;
using backend.Domains.Categories;
using backend.Domains.Movimentations;
using backend.Domains.Products;
using backend.Domains.Users;
using backend.Shared;
using backend.Shared.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public static class AppConfiguration
{
    public static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IMovimentationRepository, MovimentationRepository>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticateService>();
        builder.Services.AddScoped<IMovimentationService, MovimentationService>();
        builder.Services.AddSingleton<QueueService>();
        builder.Services.AddSingleton<WebSocketService>();
    }

    public static void AddAuthentication(WebApplicationBuilder builder)
    {

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
             .AddJwtBearer(options =>
             {

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];
                         if (!string.IsNullOrEmpty(accessToken) &&
                             (context.HttpContext.WebSockets.IsWebSocketRequest || context.HttpContext.Response.StatusCode == 200))
                         {
                             context.Token = accessToken;
                         }
                         return Task.CompletedTask;
                     }
                 };
             });

    }

    public static void AddCache(WebApplicationBuilder builder)
    {
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName");
        });

        builder.Services.Configure<DistributedCacheEntryOptions>(options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
        });
    }
    public static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor, insira um token válido",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });

    }


    public static void AddWebSocket(WebApplication app)
    {
        var webSocketOptions = new WebSocketOptions
        {

            KeepAliveInterval = TimeSpan.FromSeconds(120),
            ReceiveBufferSize = 4 * 1024,
            AllowedOrigins = { "*" }
        };

        app.UseWebSockets(webSocketOptions);

        app.Use(async (context, next) =>
        {
            
            if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
            {
                var origin = context.Request.Headers["Origin"];
                if (!string.IsNullOrEmpty(origin))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                }

            }
            await next();
        });

    }

}
