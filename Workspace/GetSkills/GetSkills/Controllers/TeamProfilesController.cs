using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GetSkills.Models;

namespace GetSkills.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeamProfilesController : Controller
    {
        private GetSkillsEntities db = new GetSkillsEntities();

        // GET: TeamProfiles
        // Team profile index page for administrators
        // [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            List<profile> profileList = (from pro in db.profile where pro.status == 1 select pro).ToList();
            List<ProfileIndexViewModel> viewList = new List<ProfileIndexViewModel>();
            foreach (var pro in profileList)
            {
                ProfileIndexViewModel tp = new ProfileIndexViewModel();
                tp.teamProfile = pro;
                viewList.Add(tp);
            }
            return View(viewList.ToList());
        }

        // GET: TeamProfiles
        // Team profile page for clients
        [AllowAnonymous]
        public ActionResult List()
        {
            List<profile> profileList = (from pro in db.profile where pro.status == 1 select pro).ToList();
            List<ProfileIndexViewModel> viewList = new List<ProfileIndexViewModel>();
            foreach (var pro in profileList)
            {
                ProfileIndexViewModel tp = new ProfileIndexViewModel();
                tp.teamProfile = pro;
                viewList.Add(tp);
            }
            return View(viewList.ToList());
        }

        // GET: TeamProfiles/Details/5
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile profile = db.profile.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: TeamProfiles/Create
        // Team profile create page for administrators
        // [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            ProfileIndexViewModel editView = new ProfileIndexViewModel();

            editView.teamProfile = new profile();
            editView.teamProfile.pic = "~/Images/no_image.jpg";

            return View(editView);
        }

        // POST: TeamProfiles/Create
        // Team profile create page for administrators
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProfileIndexViewModel editView)
        {
            if (ModelState.IsValid)
            {
                profile pro = editView.teamProfile;

                var validImageTypes = new string[]
                {
                "image/gif",
                "image/jpeg",
                "image/jpg",
                "image/png"
                };

                if (editView.picFile != null && editView.picFile.ContentLength > 0)
                {
                    if (!validImageTypes.Contains(editView.picFile.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                    }

                    var uploadDir = "~/images/teamprofile";
                    var newFileName = String.Format("{0}_{1}_{2}", "Profile", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetFileName(editView.picFile.FileName));
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), newFileName);
                    editView.picFile.SaveAs(imagePath);

                    var imageUrl = Path.Combine(uploadDir, Path.GetFileName(imagePath));
                    pro.pic = "~/images/teamprofile/" + newFileName;
                }

                pro.status = 1;
                db.profile.Add(pro);
                await db.SaveChangesAsync();            

                return RedirectToAction("Index");
            }

            return View(editView);
        }

        // GET: TeamProfiles/Edit/5
        // Team profile edit page for administrators
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile pro = await db.profile.FindAsync(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            else if (pro.status == 0)
            {
                return HttpNotFound();
            }

            ProfileIndexViewModel editView = new ProfileIndexViewModel();
            editView.teamProfile = pro;

            return View(editView);
        }

        // POST: TeamProfiles/Edit/5
        // Team profile edit page for administrators
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProfileIndexViewModel editView)
        {
            if (ModelState.IsValid)
            {
                profile pro = editView.teamProfile;

                var validImageTypes = new string[]
                {
                "image/gif",
                "image/jpeg",
                "image/jpg",
                "image/png"
                };

                if (editView.picFile != null && editView.picFile.ContentLength > 0)
                {
                    if (!validImageTypes.Contains(editView.picFile.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                    }

                    var uploadDir = "~/images/teamprofile";
                    var newFileName = String.Format("{0}_{1}_{2}", "Profile", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetFileName(editView.picFile.FileName));
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), newFileName);
                    editView.picFile.SaveAs(imagePath);

                    var imageUrl = Path.Combine(uploadDir, Path.GetFileName(imagePath));
                    pro.pic = "~/images/teamprofile/" + newFileName;
                }

                db.Entry(pro).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(editView);
        }

        // GET: TeamProfiles/Delete/5
        // Team profile delete page for administrators
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile pro = await db.profile.FindAsync(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            else if (pro.status == 0)
            {
                return HttpNotFound();
            }

            ProfileIndexViewModel editView = new ProfileIndexViewModel();
            editView.teamProfile = pro;

            return View(editView);
        }

        // POST: TeamProfiles/Delete/5
        // Team profile delete page for administrators
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            profile pro = await db.profile.FindAsync(id);
            pro.status = 0;
            db.Entry(pro).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
