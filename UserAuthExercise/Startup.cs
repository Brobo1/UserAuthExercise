using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserAuthExercise.Data;

namespace UserAuthExercise;

public class Startup {
	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services) {
		services.AddDbContext<ApplicationDbContext>(options =>
														options.UseSqlite(
															Configuration.GetConnectionString("DefaultConnection")));
		services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
		// Other service configurations...
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		} else {
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}
		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
		});

		// Call the seed method
		SeedRoles(serviceProvider).Wait();
	}

	private async Task SeedRoles(IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		var roleExists = await roleManager.RoleExistsAsync("Admin");
		if (!roleExists)
		{
			await roleManager.CreateAsync(new IdentityRole("Admin"));
		}
		else
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
			var firstUser   = await userManager.Users.FirstOrDefaultAsync();
			if(firstUser != null)
			{
				var isInRole = await userManager.IsInRoleAsync(firstUser, "Admin");
				if (!isInRole)
				{
					await userManager.AddToRoleAsync(firstUser, "Admin");
				}
			}
		}
	}
}