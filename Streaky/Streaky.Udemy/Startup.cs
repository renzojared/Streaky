﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Filters;
using Streaky.Udemy.Middlewares;
using Streaky.Udemy.Services;

namespace Streaky.Udemy;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) //resolucion de una dependencia
    {

        services.AddControllers(opt =>
        {
            opt.Filters.Add(typeof(ExceptionFiler));
        })
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
        //Ignorar ciclos author con libro y libro con author


        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

        //services.AddTransient<IService, ServiceA>(); //inyeccion a interfaces || funcion que realiza algo y retorna (no ocupa estado)
        services.AddTransient<IService, ServiceA>(); //inyeccion a interfaces || Para data en memoria por ejem
        //services.AddScoped<IService, ServiceA>(); //inyeccion a interfaces || AplicationDbContext
        //services.AddTransient<ServiceA>();

        services.AddTransient<ServiceTransient>();
        services.AddScoped<ServiceScoped>();
        services.AddSingleton<ServiceSingleton>();
        services.AddTransient<ActionFilter>();
        services.AddHostedService<WriteOnFile>();


        services.AddResponseCaching();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AuthorsAPI", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        //app.UseMiddleware<LogResponseHttpMiddleware>();
        app.UseLogResponseHttp();

        app.Map("/route1", app =>
        {
            app.Run(async contex =>
            {
                await contex.Response.WriteAsync("Estoy interceptando el middleware");
            });
        });

        if (env.IsDevelopment()) //Solo para ambiente desarrollo
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","Streaky.Udemy v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseResponseCaching();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

