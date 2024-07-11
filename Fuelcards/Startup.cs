using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;


namespace Fuelcards
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddMicrosoftIdentityUI();


            services.AddRazorPages()
                .AddMicrosoftIdentityUI();


            // Register your database contexts here
           /* services.AddDbContext<TradingContext>(options =>
                 options.UseNpgsql(Configuration.GetConnectionString("Trading")));

            services.AddDbContext<PricesContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PortlandPricesDb")));

            services.AddDbContext<CDataContext>(options =>
               options.UseNpgsql(Configuration.GetConnectionString("PortlandCDataDb")));*/

            // Register your IQueries implementation

            services.AddTransient<Microsoft.Graph.GraphServiceClient>();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Homepage}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
