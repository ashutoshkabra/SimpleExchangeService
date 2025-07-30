#region Using Namespaces

using ExchangeServiceAPI.Models;
using ExchangeServiceAPI.Services;

#endregion

namespace ExchangeServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Bind config section
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

            // Configure DI for application services
            builder.Services.AddScoped<IForexServices, ForexServices>();
            builder.Services.AddHttpClient<IForexServices, ForexServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}