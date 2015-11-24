namespace GetSkills.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<GetSkills.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GetSkills.Models.ApplicationDbContext";
        }

        protected override void Seed(GetSkills.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            AddAdminAccount(context, "ysq.evilmax@qq.com", "ai3563623", "Admin");
            AddAdminAccount(context, "getskills@outlook.com", "yogi1234", "Admin");
        }

        public void AddAdminAccount(GetSkills.Models.ApplicationDbContext context, string email, string psw, string roleName)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!rm.RoleExists(roleName))
            {
                rm.Create(new IdentityRole(roleName));
            }

            if (um.Find(email, psw) == null)
            {
                var user = new ApplicationUser { UserName = email, Email = email };
                um.Create(user, psw);
                um.AddToRole(user.Id, roleName);
            }
        }
    }
}
