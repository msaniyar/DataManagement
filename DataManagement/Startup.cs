using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataManagement.Models;
using DataManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace DataManagement
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DataManagementContext>(item => item.UseSqlServer
                (Configuration.GetConnectionString("DBConnectionString")));
            services.AddScoped<IDataControl, DataControl>();
            services.AddCors(option => option.AddPolicy("DataManagementPolicy", builder => {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

            }));

            services
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(
                    options =>
                    {
                        options.Realm = "My Application";
                        options.Events = new BasicAuthenticationEvents
                        {
                            OnValidatePrincipal = context =>
                            {

                                if ((context.UserName.ToLower() != Configuration.GetSection("UserName").Value) || (context.Password != Configuration.GetSection("Password").Value))
                                    return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name,
                                        context.UserName,
                                        context.Options.ClaimsIssuer)
                                };

                                var ticket = new AuthenticationTicket(
                                    new ClaimsPrincipal(new ClaimsIdentity(
                                        claims,
                                        BasicAuthenticationDefaults.AuthenticationScheme)),
                                    new Microsoft.AspNetCore.Authentication.AuthenticationProperties(),
                                    BasicAuthenticationDefaults.AuthenticationScheme);

                                return Task.FromResult(AuthenticateResult.Success(ticket));

                            }
                        };
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseMvc();
            app.UseCors("DataManagementPolicy");

        }
    }
}
