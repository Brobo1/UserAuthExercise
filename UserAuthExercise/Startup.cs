using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserAuthExercise.Data;

namespace UserAuthExercise;

public class Startup {
	public IConfiguration Configuration { get; set; }

	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services) {
		services.AddDbContext<ApplicationDbContext>(options =>
														options.UseSqlite(
															Configuration.GetConnectionString("DefaultConnection")));
		services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>() 
				.AddEntityFrameworkStores<ApplicationDbContext>();

	}
}