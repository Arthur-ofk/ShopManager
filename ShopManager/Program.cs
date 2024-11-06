
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();
            
            builder.Configuration.AddJsonFile("ocelot.json",optional:false , reloadOnChange:true);
            
             
            builder.Services.AddSingleton<IDefinedAggregator, UserOrdersAggregator>();
            
            builder.Services.AddEndpointsApiExplorer();
          
            builder.Services.AddSwaggerGen();
           
            builder.Services.AddControllers();

            builder.Services.AddOcelot();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway");
                    c.SwaggerEndpoint("/orders/api/swagger/v1/swagger.json", "Order Service");
                    c.SwaggerEndpoint("/users/api/swagger/v1/swagger.json", "User Service");
                    
                    c.SwaggerEndpoint("/items/api/swagger/v1/swagger.json", "Item Service");

                    c.RoutePrefix = "swagger"; 
                });
            }
           

            app.UseHttpsRedirection();
            app.MapControllers();
            try
            {
                app.UseOcelot().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Ocelot: {ex}");
            }
            app.Run();

            app.UseAuthorization();


            
        }
    }
}
