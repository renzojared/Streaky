using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Streaky.Udemy.Filters;
using Streaky.Udemy.Middlewares;
using Streaky.Udemy.Services;
using Streaky.Udemy.Utilities;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Streaky.Udemy;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) //resolucion de una dependencia
    {

        services.AddControllers(opt =>
        {
            opt.Filters.Add(typeof(ExceptionFiler));
            opt.Conventions.Add(new SwaggerGroupByVersion());
        })
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();
        //Ignorar ciclos author con libro y libro con author

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(op => op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["keyJwty"])),
                ClockSkew = TimeSpan.Zero
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AuthorsAPI",
                Version = "v1",
                Description = "Web api to work with authors and book",
                Contact = new OpenApiContact
                {
                    Email = "renzojared_lm@hotmail.com",
                    Name = "Renzo Leon",
                    Url = new Uri("https://github.com/renzojared")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "AuthorsAPI", Version = "v2" });
            c.OperationFilter<AddParameterHATEOAS>();
            c.OperationFilter<AddParameterXVersion>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            //fortokens https://jwt.io/

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });

            var fileXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var routeXML = Path.Combine(AppContext.BaseDirectory, fileXML);
            c.IncludeXmlComments(routeXML);
        });

        services.AddAutoMapper(typeof(Startup));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization(op =>
        {
            op.AddPolicy("IsAdmin", pol => pol.RequireClaim("isAdmin"));
            op.AddPolicy("IsSales", pol => pol.RequireClaim("isSales"));
        });

        services.AddDataProtection();

        services.AddTransient<HashService>();

        services.AddCors(op =>
        {
            op.AddDefaultPolicy(b =>
            {
                b.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
                .WithExposedHeaders(new string[] { "quantityTotalRecords" }); //totest : https://www.apirequest.io
                //.WithExposedHeaders();
            });
        });

        services.AddTransient<GenerateLinks>();
        services.AddTransient<HATEOASAuthorFilterAttribute>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        app.UseLogResponseHttp();

        if (env.IsDevelopment()) //Solo para ambiente desarrollo
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Streaky.Udemy v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Streaky.Udemy v2");
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

