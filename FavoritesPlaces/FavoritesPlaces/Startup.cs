using AutoMapper;
using AutoMapper.Configuration;
using TwenyTwoRT.EfStuff;
using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.EfStuff.Repository.IRepository;
using TwenyTwoRT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TwenyTwoRT
{
    public class Startup
    {
        public const string AuthMethod = "RTCookie";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=22RTWeb;Trusted_Connection=True";
               // Configuration.GetValue<string>("connectionString");
            services.AddDbContext<SpaceDbContext>(
                x => x.UseSqlServer(connectionString));
            services.AddControllersWithViews();

            services.AddAuthentication(AuthMethod)
                .AddCookie(AuthMethod, config =>
                {
                    config.Cookie.Name = "RTCookie";
                    config.LoginPath = "User/Login";
                    config.AccessDeniedPath = "User/AccessDenied";
                });

            RegisterMapper(services);
            RegistrationRepositories(services);
            RegisterService(services);
        }
        private void RegisterService(IServiceCollection services)
        {
            services.AddScoped<UserService>(diContainer =>
               new UserService(
                   diContainer.GetService<IUserRepository>(),
                   diContainer.GetService<IHttpContextAccessor>()
               ));
        }
        private void RegistrationRepositories(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes();

            foreach (var iRepo in types.Where(type =>
                type.IsInterface
                && type.GetInterfaces()
                    .Any(x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(IBaseRepository<>))
                ))
            {
                var realization = types.Single(x => x.GetInterfaces().Contains(iRepo));
                services.AddScoped(
                    iRepo,
                    diContainer => ConstructorExecutor(realization, diContainer));
            }
        }
       
        private void RegisterMapper(IServiceCollection services)
        {
            var configExpression = new MapperConfigurationExpression();

            configExpression.CreateMap<User, RegistrationViewModel>();

            var mapperConfiguration = new MapperConfiguration(configExpression);
            var mapper = new Mapper(mapperConfiguration);
            services.AddScoped<IMapper>(c => mapper);
            

            
        }
        private object ConstructorExecutor(Type realization, IServiceProvider diContainer)
        {
            var constructor = realization.GetConstructors()[0];
            var paramInfoes = constructor.GetParameters();

            var paramValues = new object[paramInfoes.Length];
            for (int i = 0; i < paramInfoes.Length; i++)
            {
                var paramInfo = paramInfoes[i];
                var paramValue = diContainer.GetService(paramInfo.ParameterType);
                paramValues[i] = paramValue;
            }

            var answer = constructor.Invoke(paramValues);
            return answer;
        }
        public void MapBoth<Type1, Type2>(MapperConfigurationExpression configExpression)
        {
            configExpression.CreateMap<Type1, Type2>();
            configExpression.CreateMap<Type2, Type1>();
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
