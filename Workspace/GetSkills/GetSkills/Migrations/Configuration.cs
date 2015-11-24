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

            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            rm.Create(new IdentityRole("Admin"));

            var user = new ApplicationUser { UserName = "ysq.evilmax@qq.com", Email = "ysq.evilmax@qq.com" };
            um.Create(user, "ai3563623");
            um.AddToRole(user.Id, "Admin");
        }
    }
}
