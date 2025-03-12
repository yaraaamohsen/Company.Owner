using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Company.Owner.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependacy Injection For departmentRepository
            builder.Services.AddScoped<IEmployeeRemository, EmployeeRepository>(); // Allow Dependacy Injection For departmentRepository
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // As GetConnectionString built in
                //options.UseSqlServer(builder.Configuration["DefaultConnection"]); // in case it's user function


            }); // Allow Dependacy Injection For CompanyDbContext


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=employee}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
