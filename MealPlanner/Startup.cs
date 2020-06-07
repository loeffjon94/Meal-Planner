using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MealPlanner.Data.Contexts;
using MealPlanner.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner
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
            services.AddControllersWithViews();
            services.AddProgressiveWebApp();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddOptions();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MealPlannerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MealPlannerContext")));

            services.AddScoped<IngredientService>();
            services.AddScoped<MealsService>();
            services.AddScoped<PlanningService>();
            services.AddScoped<RecipeService>();
            services.AddScoped<ShoppingService>();
            services.AddScoped<MealGroupService>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                    .AddCookie(opts =>
                    {
                        opts.Cookie.SameSite = SameSiteMode.None;
                    })
                    .AddOpenIdConnect(opts =>
                    {
                        Configuration.GetSection("OpenIdConnect").Bind(opts);
                        opts.Events = new OpenIdConnectEvents
                        {
                            OnAuthorizationCodeReceived = ctx =>
                            {
                                return Task.CompletedTask;
                            }
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            //app.UseDeveloperExceptionPage();
            //app.UseBrowserLink();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
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
