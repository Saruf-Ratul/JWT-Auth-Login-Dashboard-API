// Startup.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.OpenApi.Models;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                builder => builder.WithOrigins("http://localhost:4200") // Add your Angular app URL
                                                .AllowAnyHeader()
                                                .AllowCredentials()
                                                .AllowAnyMethod());

        });

        // Add Swagger
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi", Version = "v1" });
            });


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", jwtBearerOptions =>
        {
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.FromMinutes(5)
            };
        });



        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowOrigin");

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
        });

    }
}
