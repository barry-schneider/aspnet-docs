using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebHTTPS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region snippet2
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
		// if the web server is behind another device then non-standard port may need to be needed, for example
		// options.HttpsPort = 5001;
				
		//Temporary Redirect https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/307
		options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect; 
				
                //Permanent Redirect https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/308
		//options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect; 
            });            
        }
        #endregion

        #region snippet1
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
        #endregion
    }
}
