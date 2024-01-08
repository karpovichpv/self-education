﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Utility;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            Configuration.GetConnectionString("DefaultConnection")));

        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddDefaultTokenProviders()
            .AddDefaultUI()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddTransient<IEmailSender, EmailSender>();

        services.AddHttpContextAccessor();
        services.AddSession(Options =>
        {
            Options.IdleTimeout = TimeSpan.FromMinutes(10);
            Options.Cookie.HttpOnly = true;
            Options.Cookie.IsEssential = true;
        });
        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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
        app.UseSession();
        app.UseEndpoints(endPoints =>
        {
            endPoints.MapRazorPages();
            endPoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}