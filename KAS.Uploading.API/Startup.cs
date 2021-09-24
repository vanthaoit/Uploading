using AutoMapper;
using KAS.Uploading.API.Configuration;
using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.BusinessFunctions.Services;
using KAS.Uploading.DataAccess;
using KAS.Uploading.DataAccess.Implements;
using KAS.Uploading.DataAccess.Interfaces;
using KAS.Uploading.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KAS.Uploading.API
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies()
                   .UseSqlServer(Configuration.GetConnectionString("AppDbConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddCors(o => o.AddPolicy("KASCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            //services.AddAutoMapper();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 15;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            // Add application services.
            //services.AddAutoMapper();

            //services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddScoped(typeof(IEFCommonService<,>), typeof(EFCommonService<,>));

            services.AddScoped(typeof(IEFRepository<,>), typeof(EFRepository<,>));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TruongThinh API",
                    Description = "The API of  TruongThinh Version 1.0"
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });

          
            services.AddAuthorization(options=> {
                options.AddPolicy("ApiPolicySuperAdmin",policy=> {
                    policy.RequireClaim("scope","super_admin");
                });
                options.AddPolicy("ApiPolicyAdmin", policy => {
                    policy.RequireClaim("scope", "admin");
                });
            });
            services.AddAuthentication().AddIdentityServerAuthentication(options=> {
                options.Authority = Configuration["Identity:Authority"];
                options.RequireHttpsMetadata = false;
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            //OR
            services.AddControllers(options => options.EnableEndpointRouting = false);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("KASCorsPolicy");
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = new[] { new CultureInfo("en"), new CultureInfo("vi") },
                SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("vi") },
                RequestCultureProviders = new IRequestCultureProvider[] {
                    new QueryStringRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                },
            });
            
            app.UseIdentityServer();
            //app.UseAuthentication();
            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TruongThinh API V1.0 Development");
                });
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TruongThinh API V1.0 Staging");
                });
            }

            app.UseAuthentication();
           
            //app.UseMvc();
            //dbInitializer.Seed().Wait();
        }
    }
}