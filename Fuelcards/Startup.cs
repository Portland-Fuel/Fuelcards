using DataAccess.Cdata;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using DataAccess.Repositorys.UnitOfWork;
using DataAccess.Tickets;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
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
            services.AddScoped<IFuelcardUnitOfWork, FuelcardUnitOfWork>();
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddDbContext<CDataContext>(options => options.UseNpgsql(
                   Configuration.GetConnectionString("Cdata")));
            services.AddDbContext<FuelcardsContext>(options => options.UseNpgsql(
                   Configuration.GetConnectionString("FuelcardDb")));
            services.AddDbContext<IfuelsContext>(options => options.UseNpgsql(
                   Configuration.GetConnectionString("Ifuels")));
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
            services.AddScoped<IQueriesRepository, QueriesRepository>();
            services.AddControllers();
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
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
