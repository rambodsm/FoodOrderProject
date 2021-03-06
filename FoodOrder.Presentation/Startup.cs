using FoodOrder.Common.SiteSettings;
using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Infrastructure.Repositories.User;
using FoodOrder.Infrastructure.UnitOfWork;
using FoodOrder.Service.Contracts;
using FoodOrder.Service.CustomMapping;
using FoodOrder.Service.Services;
using FoodOrder.WebFramework.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodOrder.Presentation
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.AddDbContext(Configuration);
            services.AddCustomIdentity(_siteSetting.IdentitySettings);
            services.AddMinimalMvc();
            services.AddSwaggerGen(option=> {option.EnableAnnotations();});
            services.AddAutoMapper(typeof(AutomapperProfileConfiguration).Assembly);
            services.AddJwtAuthentication(_siteSetting.JwtSettings);
            #region IoC
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //    .RequireAuthorization(); Apply AuthorizeFilter as global filter to all endpoints
            });
        }
    }
}
