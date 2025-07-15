
using Microsoft.EntityFrameworkCore;
using MovieData.Services;
using MovieData.Data;

namespace MovieApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MovieContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext") ?? throw new InvalidOperationException("Connection string 'MovieContext' not found.")));

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());


            builder.Services.AddControllers()
            .AddNewtonsoftJson();
          

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
            }
            );

            builder.Services.AddHostedService<DataSeedHostingService>();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
              //  await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
