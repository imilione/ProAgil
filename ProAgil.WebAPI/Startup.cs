using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
using ProAgil.Repository;

namespace ProAgil.WebAPI
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
      services.AddDbContext<ProAgilContext>(
          x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
      );

      //CONFIGURAÇÃO EM RELAÇÃO AO PASSWORD
      IdentityBuilder builder = services.AddIdentityCore<User>(options =>
        {
          options.Password.RequireDigit = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireLowercase = false;
          options.Password.RequireUppercase = false;
          options.Password.RequiredLength = 4;
        }
      );

      //CONFIG CONTEXTO/ROLE: QUEM VAI CONTROLAR O CADASTRO DO USUÁRIO
      builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
      builder.AddEntityFrameworkStores<ProAgilContext>();
      builder.AddRoleValidator<RoleValidator<Role>>();
      builder.AddRoleManager<RoleManager<Role>>();
      builder.AddSignInManager<SignInManager<User>>();

      //CONFIGURAÇÃO DE JWT
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                //APONTAMENTO PARA O SECRET
                .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
              ValidateIssuer = false,
              ValidateAudience = false
            };
          }
        );

      //POLÍTICA DE AUTENTICAÇÃO DE TODAS AS CONTROLLERS
      //ESTOU DIZENDO QUE TODAS AS CONTROLLERS PRECISAM SER AUTORIZADAS
      services.AddMvc(options =>
        {
          var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
          options.Filters.Add(new AuthorizeFilter(policy));
        }
      )
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
      //EVITA LOOPS 
      .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

      services.AddScoped<IProAgilRepository, ProAgilRepository>();
      services.AddAutoMapper();
      services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      //app.UseHttpsRedirection();
      app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      app.UseStaticFiles(new StaticFileOptions()
      {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
        RequestPath = new PathString("/Resources")
      }); //to allow images to the
      app.UseMvc();
    }
  }
}
