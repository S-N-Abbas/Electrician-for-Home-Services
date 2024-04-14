using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Electrician_for_Home_Services.Models;

[assembly: OwinStartupAttribute(typeof(Electrician_for_Home_Services.Startup))]
namespace Electrician_for_Home_Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUser();
        }

        private void CreateRolesAndUser()
        {
            ApplicationDbContext ADC = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ADC));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ADC));

            // Creating Admin Role and registring a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // Creating Admin Role  
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Creating Admin User (Super User) for the website
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@E4HS.com";

                // Admin Default Password
                string userPWD = "Admin@E4HS";

                var chkUser = UserManager.Create(user, userPWD);

                // Assigning Admin Role to this user   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
