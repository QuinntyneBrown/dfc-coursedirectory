using Dfc.CourseDirectory.Services.Interfaces.ProviderService;
using Dfc.CourseDirectory.Services.ProviderService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Dfc.CourseDirectory.Services.Tests
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            
            services.AddOptions();

            services.AddTransient((provider) => new HttpClient());

            services.Configure<ProviderServiceSettings>(Configuration.GetSection(nameof(ProviderServiceSettings)));
            services.AddScoped<IProviderService, ProviderService.ProviderService>();
        }
    }
}
