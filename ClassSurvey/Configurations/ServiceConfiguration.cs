using ClassSurvey.Contexts;
using ClassSurvey.Entities;
using ClassSurvey.Services;
using Microsoft.AspNetCore.Identity;
using Polly.Extensions.Http;
using Polly;
using Microsoft.EntityFrameworkCore;
using ClassSurvey.Helpers;

namespace ClassSurvey.Configurations
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddDefaultIdentity<UserEntity>(x =>
            {
                x.User.RequireUniqueEmail = true;
                x.SignIn.RequireConfirmedAccount = false;
                x.Password.RequiredLength = 8;
                x.Lockout.MaxFailedAccessAttempts = 3;
            })
                      .AddRoles<IdentityRole>()
                      .AddEntityFrameworkStores<DataContext>();

            services.AddControllersWithViews();

            services.AddHttpClient("LimitedClient")
                .AddPolicyHandler(GetRateLimitPolicy());

            static IAsyncPolicy<HttpResponseMessage> GetRateLimitPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            }

            services.AddDbContext<DataContext>(x => x.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddScoped<SurveyService>();
            services.AddScoped<DataAggregationHelper>();
            services.AddScoped<AdminService>();
            services.AddScoped<JWTService>();
        }
    }
}
