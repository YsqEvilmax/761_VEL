﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace GetSkills.Models
{
    public class SuccessStoryController : Controller
    {
        private GetSkillsEntities db = new GetSkillsEntities();

        // GET: SuccessStory
        public async Task<ActionResult> Index(string sortOrder)
        {

            List<success_story> storyList = (from ss in db.success_story where ss.status == 1 select ss).ToList();

            List<success_story> outList;

            ViewBag.IDSortParm = sortOrder;
            ViewBag.OrderSortParm = sortOrder;

            if ("ID_asc".Equals(sortOrder))
            {
                //outList = db.success_story.OrderBy(e => e.success_story_id).ToList();
                outList = storyList.OrderBy(e => e.success_story_id).ToList();
            }
            else if ("ID_desc".Equals(sortOrder))
            {
                outList = storyList.OrderByDescending(e => e.success_story_id).ToList();
            }
            else if ("Order_desc".Equals(sortOrder))
            {
                outList = storyList.OrderByDescending(e => e.sort_number).ToList();
            }
            else if ("Order_asc".Equals(sortOrder))
            {
                outList = storyList.OrderBy(e => e.sort_number).ToList();
            }
            else
            {
                ViewBag.OrderSortParm = "Order_asc";
                outList = storyList.OrderBy(e => e.sort_number).ToList();
            }

            List<StoryIndexViewModel> viewList = new List<StoryIndexViewModel>();
            foreach (var rec in outList)
            {
                StoryIndexViewModel st = new StoryIndexViewModel();
                st.successStory = rec;

                st.categoryList = (from cat in db.success_story_category
                                   join cam in db.categories on cat.category_id equals cam.category_id
                                   where cat.success_story_id == rec.success_story_id && cat.status == 1
                                   select new StoryCategoryViewModel
                                   {
                                       story_category_id = cat.story_category_id,
                                       success_story_id = cat.success_story_id,
                                       category_id = cat.category_id,
                                       category_name = cam.category_name,
                                       status = cat.status
                                   }).ToList();

                st.coursesList = (from ssc in db.success_story_courses
                                  join cos in db.courses on ssc.course_id equals cos.course_id
                                  where ssc.success_story_id == rec.success_story_id && ssc.status == 1
                                  select new StoryCourseViewModel
                                  {
                                      story_course_id = ssc.story_course_id,
                                      success_story_id = ssc.success_story_id,
                                      course_id = ssc.course_id,
                                      course_name = cos.course_name,
                                      status = ssc.status
                                  }).ToList();

                viewList.Add(st);
            }

            //return View(await db.success_story.ToListAsync());
            return View(viewList.ToList());
        }

        // LIST: SuccessStory
        public async Task<ActionResult> List()
        {
            return View(await db.success_story.ToListAsync());
        }

        // GET: SuccessStory/Details/5
        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoryIndexViewModel stViewModel = new StoryIndexViewModel();
            stViewModel.successStory = await db.success_story.FindAsync(id);
            if (stViewModel.successStory == null)
            {
                return HttpNotFound();
            }
            stViewModel.categoryList = (from cat in db.success_story_category
                                        join cam in db.categories on cat.category_id equals cam.category_id
                                        where cat.success_story_id == stViewModel.successStory.success_story_id && cat.status == 1
                                        select new StoryCategoryViewModel
                                        {
                                            story_category_id = cat.story_category_id,
                                            success_story_id = cat.success_story_id,
                                            category_id = cat.category_id,
                                            category_name = cam.category_name,
                                            status = cat.status
                                        }).ToList();

            stViewModel.coursesList = (from ssc in db.success_story_courses
                                       join cos in db.courses on ssc.course_id equals cos.course_id
                                       where ssc.success_story_id == stViewModel.successStory.success_story_id && ssc.status == 1
                                       select new StoryCourseViewModel
                                       {
                                           story_course_id = ssc.story_course_id,
                                           success_story_id = ssc.success_story_id,
                                           course_id = ssc.course_id,
                                           course_name = cos.course_name,
                                           status = ssc.status
                                       }).ToList();
            stViewModel.courses = (from ssc in db.success_story_courses
                                   join cos in db.courses on ssc.course_id equals cos.course_id
                                   where ssc.success_story_id == stViewModel.successStory.success_story_id && ssc.status == 1
                                   select new CourseViewModel
                                   {
                                       course_id = cos.course_id,
                                       course_name = cos.course_name,
                                       course_code = cos.course_code,
                                       subject_id = cos.subject_id,
                                       topic_id = cos.topic_id,
                                       description = cos.description,
                                       price = cos.price,
                                       status = cos.status
                                   }).ToList();
            return View(stViewModel);
        }

        // GET: SuccessStory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SuccessStory/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "success_story_id,user_name,pic,sort_number,brief,detail_description,status")] success_story success_story)
        {
            if (ModelState.IsValid)
            {
                db.success_story.Add(success_story);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(success_story);
        }

        // GET: SuccessStory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            success_story story = await db.success_story.FindAsync(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            else if (story.status == 0)
            {
                return HttpNotFound();
            }

            StoryIndexViewModel editView = new StoryIndexViewModel();
            editView.successStory = story;

            editView.allCategoryList = (from ca in db.categories
                                        from ssc in db.success_story_category
                                             .Where(ssc => ssc.category_id == ca.category_id
                                                  && ssc.success_story_id == story.success_story_id
                                                  && ssc.status == 1).DefaultIfEmpty()
                                        select new CategoryCheckBoxModel
                                        {
                                            category_id = ca.category_id,
                                            category_name = ca.category_name,
                                            story_category_id = (ssc == null ? 0 : ssc.story_category_id),
                                            isSelected = (ssc == null ? false : true)
                                        }).ToList();

            editView.allCourseList = (from co in db.courses
                                      from ssc in db.success_story_courses
                                           .Where(ssc => ssc.course_id == co.course_id
                                                && ssc.success_story_id == story.success_story_id
                                                && ssc.status == 1).DefaultIfEmpty()
                                      select new CourseCheckboxModel
                                      {
                                          course_id = co.course_id,
                                          course_name = co.course_name,
                                          story_course_id = (ssc == null ? 0 : ssc.story_course_id),
                                          isSelected = (ssc == null ? false : true)
                                      }).ToList();

            return View(editView);
        }

        // POST: SuccessStory/Edit/5
        // For deatail, http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StoryIndexViewModel editView)
        {

            if (ModelState.IsValid)
            {
                success_story story = editView.successStory;

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

                    var uploadDir = "~/images";
                    var newFileName = String.Format("{0}_{1}_{2}", "Story", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetFileName(editView.picFile.FileName));
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), newFileName);
                    editView.picFile.SaveAs(imagePath);

                    var imageUrl = Path.Combine(uploadDir, Path.GetFileName(imagePath));
                    story.pic = "~/images/" + newFileName;
                }

                db.Entry(story).State = EntityState.Modified;

                foreach (CategoryCheckBoxModel cat in editView.allCategoryList)
                {
                    if (cat.story_category_id == 0 && cat.isSelected == true)
                    {
                        success_story_category newSsc = new success_story_category();
                        newSsc.success_story_id = story.success_story_id;
                        newSsc.category_id = cat.category_id;
                        newSsc.status = 1;
                        db.success_story_category.Add(newSsc);
                    }
                    if (cat.story_category_id != 0 && cat.isSelected == false)
                    {
                        success_story_category updateSsc = new success_story_category();
                        updateSsc.story_category_id = cat.story_category_id;
                        updateSsc.success_story_id = story.success_story_id;
                        updateSsc.category_id = cat.category_id;
                        updateSsc.status = 0;
                        db.Entry(updateSsc).State = EntityState.Modified;
                    }
                }

                foreach (CourseCheckboxModel cor in editView.allCourseList)
                {
                    if (cor.story_course_id == 0 && cor.isSelected == true)
                    {
                        success_story_courses newSsc = new success_story_courses();
                        newSsc.success_story_id = story.success_story_id;
                        newSsc.course_id = cor.course_id;
                        newSsc.status = 1;
                        db.success_story_courses.Add(newSsc);
                    }
                    if (cor.story_course_id != 0 && cor.isSelected == false)
                    {
                        success_story_courses updateSsc = new success_story_courses();
                        updateSsc.story_course_id = cor.story_course_id;
                        updateSsc.success_story_id = story.success_story_id;
                        updateSsc.course_id = cor.course_id;
                        updateSsc.status = 0;
                        db.Entry(updateSsc).State = EntityState.Modified;
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(editView);
        }

        // GET: SuccessStory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            success_story story = await db.success_story.FindAsync(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            else if (story.status == 0)
            {
                return HttpNotFound();
            }

            StoryIndexViewModel editView = new StoryIndexViewModel();
            editView.successStory = story;

            editView.allCategoryList = (from ca in db.categories
                                        from ssc in db.success_story_category
                                             .Where(ssc => ssc.category_id == ca.category_id
                                                  && ssc.success_story_id == story.success_story_id
                                                  && ssc.status == 1)
                                        select new CategoryCheckBoxModel
                                        {
                                            category_id = ca.category_id,
                                            category_name = ca.category_name,
                                            story_category_id = (ssc == null ? 0 : ssc.story_category_id),
                                            isSelected = (ssc == null ? false : true)
                                        }).ToList();

            editView.allCourseList = (from co in db.courses
                                      from ssc in db.success_story_courses
                                           .Where(ssc => ssc.course_id == co.course_id
                                                && ssc.success_story_id == story.success_story_id
                                                && ssc.status == 1)
                                      select new CourseCheckboxModel
                                      {
                                          course_id = co.course_id,
                                          course_name = co.course_name,
                                          story_course_id = (ssc == null ? 0 : ssc.story_course_id),
                                          isSelected = (ssc == null ? false : true)
                                      }).ToList();

            return View(editView);
        }

        // POST: SuccessStory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            success_story story = await db.success_story.FindAsync(id);

            story.status = 0;
            db.Entry(story).State = EntityState.Modified;
            //db.success_story.Remove(story);

            List<success_story_category> categoryList = (from ca in db.success_story_category
                                                         where ca.success_story_id == story.success_story_id && ca.status == 1
                                                         select ca).ToList();

            foreach (success_story_category storyCategory in categoryList)
            {
                storyCategory.status = 0;
                db.Entry(storyCategory).State = EntityState.Modified;
            }

            List<success_story_courses> courseList = (from ca in db.success_story_courses
                                                      where ca.success_story_id == story.success_story_id && ca.status == 1
                                                      select ca).ToList();

            foreach (success_story_courses storyCourse in courseList)
            {
                storyCourse.status = 0;
                db.Entry(storyCourse).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
