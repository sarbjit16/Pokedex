using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokeDex.FunTranslations.Provider;
using PokeDex.Infrastructure.Cache;
using PokeDex.Infrastructure.Http;
using PokeDex.PokeAPI.Provider;
using PokeDex.Services.Pokemons;

namespace PokeDex.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddScoped<ICacheStorage, MemoryCacheAdapter>();
            services.AddScoped<IHttpRestClient, HttpRestClient>();
            services.AddScoped<IPokeAPIProvider, PokeAPIProvider>();
            services.AddScoped<IFunTranslationsProvider, FunTranslationsProvider>();
            services.AddScoped<IPokemonService, PokemonService>();

            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
