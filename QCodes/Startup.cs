using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QCodes.Data;
using QCodes.Hubs;
using QCodes.Repository;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace QCodes
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
            services.AddControllers();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
            services.AddCors(o => o.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200").AllowCredentials();
                }));
            services.AddSignalR();
            services.AddAutoMapper(typeof(UserAndPersonRepository).Assembly);

            services.AddScoped<IQTagRepository, QTagRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();
            services.AddSingleton<InMemoryDbForUserInfo>();
            services.AddScoped<IUserAndPersonRepository, UserAndPersonRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AuthSetting:tokenKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/PrivateMessageHub") || (path.StartsWithSegments("/MessageHub"))  ) )
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };


            });




            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 6;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredUniqueChars = 0;
                option.SignIn.RequireConfirmedEmail = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors("CorsPolicy");
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/MessageHub");
                endpoints.MapHub<PrivateMessageHub>("/PrivateMessageHub");
            });
        }
    }
}
