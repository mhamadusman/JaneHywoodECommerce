using ApplicationLayer.Services;
using Core.Interfaces;
using Inftastructure.Hubs;
using Inftastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using WebApplication5.Data;
using WebApplication5.Models;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		//Removing dependencies 
		builder.Services.AddControllersWithViews();
		builder.Services.AddTransient<CartService>();
        builder.Services.AddTransient<ProductService>();
        builder.Services.AddTransient(typeof(GenericService<>));

        //Injecting services
        builder.Services.AddTransient<IProductRepository , ProductRepository>();
        builder.Services.AddTransient<ICartRepository, CartRepository>();
        builder.Services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

		//registring Hub Class --> go to Map it
		builder.Services.AddSignalR();

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));
		builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>();
		builder.Services.AddControllersWithViews();
		//add policy
		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy("admin", policy =>
				policy.RequireClaim(ClaimTypes.Email, "admin@gmail.com"));
		});
		//add session 
		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromDays(4); // Set session timeout to 4 days
			options.Cookie.HttpOnly = true; // Make the session cookie accessible only by the server
			options.Cookie.IsEssential = true; // Make the session cookie essential
		});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
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
		app.UseSession();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");
        //Mapping hub pip
        app.MapHub<NotificationHub>("/notificationHub");
        app.MapRazorPages();
		app.Run();
	}
}