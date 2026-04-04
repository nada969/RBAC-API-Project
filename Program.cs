
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RBAC_API_project.Data;
using RBAC_API_project.Repository;
using RBAC_API_project.Services;
using System.Configuration;
using System.Text;

namespace RBAC_API_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ////      Add Data and Models
            builder.Services.AddDbContext<UserDb>(options =>
                options.UseNpgsql( builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))                
                );
            ////      Add jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer( o => {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime=true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidIssuer = builder.Configuration["JWT:Is9suer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

                    };


                })
                
                ;


            /// Add repository
            builder.Services.AddScoped<IUserRepo, UserRepo>();

            // Add services 
            builder.Services.AddScoped<IUserService,UserService>();

            // JWT
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            // Add Controllers
            builder.Services.AddControllers();



            //  configuring OpenAPI & Swagger
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Police
            builder.Services.AddCors(corsOptions =>
                corsOptions.AddPolicy("MyPolice", CorsPolicyBuilder =>
                {
                    //CorsPolicyBuilder.WithOrigins("")  // for spacific domain(like: www.google.com, ...)
                    CorsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                })
            );

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();   /// html , images, ...

            // SETTING CORS POLICE
            app.UseCors("MyPolice");  /// TO ENABLE API OPEN IN EXTERNAL HTML PAGES

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
