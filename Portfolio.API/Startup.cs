using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SharePoint.Client;
using Portfolio.API.Models;

namespace Portfolio.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped((serviceProvider) =>
            {
                var spConfig = serviceProvider.GetRequiredService<IConfiguration>().GetSection("SharePointConfiguration");
                return new SharePointConfiguration
                {
                    TargetSite = spConfig.GetValue<string>("targetSite"),
                    Credentials = new SharePointOnlineCredentials(spConfig.GetValue<string>("userName"), spConfig.GetValue<string>("password"))
                };
            });

            services.AddScoped((serviceProvider) =>
            {
                var mailConfig = serviceProvider.GetRequiredService<IConfiguration>();

                var smtpClient = new SmtpClient();

                if (Environment.IsDevelopment())
                {
                    smtpClient.Host = "127.0.0.1";
                    smtpClient.Port = 25;
                }

                else
                {
                    smtpClient.Host = "smtp.sendgrid.net";
                    smtpClient.Credentials = new NetworkCredential(mailConfig.GetValue<string>("Smtp:UserName"), mailConfig.GetValue<string>("Smtp:Password"));
                }

                return smtpClient;
            });
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
        }
    }
}
