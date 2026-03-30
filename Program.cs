
using Microsoft.EntityFrameworkCore;
using RBAC_API_project.Data;
using RBAC_API_project.Repository;
using RBAC_API_project.Services;

namespace RBAC_API_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //// Add Data and Models
            builder.Services.AddDbContext<UserDb>(options =>
                options.UseNpgsql( builder.Configuration.GetConnectionString("DefaultConnection") )                
                );


            /// Add repository
            builder.Services.AddScoped<IUserRepo, UserRepo>();

            // Add services 
            builder.Services.AddScoped<IUserService,UserService>();

            // Add Controllers
            builder.Services.AddControllers();



            //  configuring OpenAPI & Swagger
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
