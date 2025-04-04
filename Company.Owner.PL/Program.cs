using AutoMapper;
using Company.Owner.BLL;
using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Data.Contexts;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Helper.EmailSetting;
using Company.Owner.PL.Helper.SmsConfig;
using Company.Owner.PL.Mapping;
using Company.Owner.PL.Services;
using Company.Owner.PL.Setting;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.Owner.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            //// To Login With Facebook
            //builder.Services.AddAuthentication(O =>
            //{
            //    O.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
            //    O.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;

            //}).AddFacebook(O => {
            //    O.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
            //    O.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
            //});

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependacy Injection For departmentRepository
            //builder.Services.AddScoped<IEmployeeRemository, EmployeeRepository>(); // Allow Dependacy Injection For departmentRepository
            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMailService, MailSrvice>();
            builder.Services.AddScoped<ITwilioService, TwilioService>();

            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.LogoutPath = "/Home/SignIn";
            });


            // To Login With Google
            builder.Services.AddAuthentication(O =>
            {
                O.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                O.DefaultScheme = IdentityConstants.ApplicationScheme;
                O.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                O.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

            }).AddGoogle(O => {
                O.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                O.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });



            // To Build Connection
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // As GetConnectionString built in
                //options.UseSqlServer(builder.Configuration["DefaultConnection"]); // in case it's user function
            }); // Allow Dependacy Injection For CompanyDbContext

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection(nameof(TwilioSetting)));


            #region Services Life Time
            // It depends On Lifetime
            //builder.Services.AddScoped();     // Create Object Life Time Per Request - Unreachable Object
            //builder.Services.AddTransient();  // Create Object Life Time Per Operation
            //builder.Services.AddSingleton();  // Create Object Life Time Per App

            builder.Services.AddScoped<IScopedService, ScopedService>();          // Per Request
            builder.Services.AddTransient<ITransientService, TransientService>(); // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per App
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=employee}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
