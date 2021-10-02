using EmailCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailWebAPI
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
            // get email config
            var emailConfig = Configuration
                .GetSection("EmailConfig")
                .Get<EmailConfig>();

            // add singleton for email config
            services.AddSingleton(emailConfig);

            // add email sender service
            services.AddScoped<ISender, Sender>();

            // add form options
            services.Configure<FormOptions>(f =>
            {
                f.MemoryBufferThreshold = int.MaxValue;
                f.MultipartBodyLengthLimit = int.MaxValue;
                f.ValueLengthLimit = int.MaxValue;
            });

            // add controllers
            services.AddControllers();
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

            // ONLY USED SO THAT OUR "LITE WEB PAGE" WILL WORK!
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
