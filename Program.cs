using Employee.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to container
            builder.Services.AddControllers();

            builder.Services.AddDbContext<EmployeeDbContext>(opt =>
                opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("empCon")));

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Development middleware
            if (app.Environment.IsDevelopment())
            {
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