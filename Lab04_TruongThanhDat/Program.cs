using CodeMegaVNPay.Services;
using Lab04_TruongThanhDat.Models;
using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Management.Storage.Fluent.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab04_TruongThanhDat
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option => { })
				.AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(option =>
			{
				option.LoginPath = $"/Identity/Account/Login";
				option.LogoutPath = $"/Identity/Account/Logout";
				option.LoginPath = $"/Identity/Account/AccessDenied";
			});
            builder.Services.AddScoped<IVnPayService, VnPayService>();

            builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
            builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
                facebookOptions.AppId = facebookAuthNSection["AppId"];
                facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
                facebookOptions.CallbackPath = "/signin-facebook";
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                googleOptions.CallbackPath = "/signin-google";

            });
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IProductRepository, EFProductRepository>();
			builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
			
			builder.Services.AddScoped<ICartService, EFCartService>();

			builder.Services.AddRazorPages();
			builder.Services.AddControllersWithViews();
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

			app.UseSession();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

			});

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");



			app.Run();
		}
	}
}