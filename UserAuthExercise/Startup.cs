using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserAuthExercise.Data;

namespace UserAuthExercise;

public class Startup {
	public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
    
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()  // This allows you to use roles.
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddControllersWithViews();
        services.AddRazorPages();
    }
}