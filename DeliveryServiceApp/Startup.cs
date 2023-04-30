using DeliveryServiceApp.Services.Implementation;
using DeliveryServiceApp.Services.Interfaces;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceData.UnitOfWork.Implementation;
using DeliveryServiceDomain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceApp
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
            services.AddScoped<IServiceAdditonalService, ServiceAdditionalService>();
            services.AddScoped<IServiceAddionalServiceShipment, ServiceAdditionalServiceShipment>();
            services.AddScoped<IServiceCustomer, ServiceCustomer>();
            services.AddScoped<IServiceShipment, ServiceShipment>();
            services.AddScoped<IServiceShipmentWeight, ServiceShipmentWeight>();

            services.AddControllersWithViews(
            ).AddJsonOptions(x => x.JsonSerializerOptions.MaxDepth = Int32.MaxValue);

            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(10));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPersonUnitOfWork, PersonUnitOfWork>();
            services.AddDbContext<DeliveryServiceContext>();
            services.AddDbContext<PersonContext>();
            services.AddScoped<IPasswordHasher<Person>, PasswordHasher<Person>>();

            services.AddIdentity<Person, IdentityRole<int>>().AddEntityFrameworkStores<PersonContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Home/AccesDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SlidingExpiration = true;
            });

  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
