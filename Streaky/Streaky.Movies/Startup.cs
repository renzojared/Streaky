using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Streaky.Movies.Helper;
using Streaky.Movies.Services;

namespace Streaky.Movies;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));//Proyecto donde se va encontrar las clases de mapeo.

        services.AddTransient<IStorageFiles, StorageFilesLocal>(); //For Local or Azure
        services.AddHttpContextAccessor();

        services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddSingleton(p =>
        new MapperConfiguration(c =>
        {
            var geometryFactory = p.GetRequiredService<GeometryFactory>();
            c.AddProfile(new AutoMapperProfiles(geometryFactory));
        }).CreateMapper());

        services.AddDbContext<ApplicationDbContext>(opt =>
        opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
        s => s.UseNetTopologySuite()));

        services.AddControllers()
            .AddNewtonsoftJson();

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                ClockSkew = TimeSpan.Zero
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebApiMovies", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(u => u.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiMovies v1"));
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(e =>
        {
            e.MapControllers();
        });
    }
}

