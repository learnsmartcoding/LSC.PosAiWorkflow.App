
using LSC.PosAiWorkflow.Api.BackgroundServices;
using LSC.PosAiWorkflow.Api.DependencyInjection;
using LSC.PosAiWorkflow.Api.Simulation;

namespace LSC.PosAiWorkflow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApi();
            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "My API";
                config.Version = "v1"; // optional, defaults to v1
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularUi", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddProjectServices(builder.Configuration);

            builder.Services.AddSingleton<SimulationDataService>();
            builder.Services.AddHostedService<CatalogBootstrapBackgroundService>();
            builder.Services.AddHostedService<OrderSimulationBackgroundService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseCors("AngularUi");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
