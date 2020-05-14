using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Json2Kafka.Services;

namespace Json2Kafka
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            // surcharge les confs classiques de 'appsettings.json' avec les variables d'environnements
            var builder = new ConfigurationBuilder();
            builder.AddConfiguration(configuration);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); //ajout du controller msg qui gère la partie API http
            services.AddHealthChecks(); //pour kubernetes health prode
            services.AddScoped<IUserService, UserService>(); //Injection de dépendance - ajout au scope appli le service qui vérifie le login mdp utilisé pour le basicAuth de cette appli
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });

        }
    }
}
