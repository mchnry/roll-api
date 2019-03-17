using Mchnry.Core.Encryption;
using Mchnry.Core.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using roll_api.Infrastructure;
using roll_api.Infrastructure.Configuration;
using roll_api.Infrastructure.Security;

namespace roll_api
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

            
            JWTConfiguration jwtConfig = new JWTConfiguration();
            
            this.Configuration.Bind("JWT", jwtConfig);

            services.AddSingleton<JWTConfiguration>(jwtConfig);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IRSAKeyProvider>((ctx) =>
            {
                JWTConfiguration c = ctx.GetService<JWTConfiguration>();
                return new MachineStoreKeyProvider(System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, c.ThumbPrint);
            });
            services.AddSingleton(new ClientHelper());
            services.AddSingleton(new JWTHelper());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseJWT();
            });

            //app.UseJWT();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
