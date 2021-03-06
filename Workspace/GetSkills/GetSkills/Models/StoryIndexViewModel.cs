namespace GetSkills.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;

    public partial class StoryIndexViewModel
    {
        public virtual success_story successStory { get; set; }
        public virtual List<StoryCategoryViewModel> categoryList { get; set; }
        public virtual List<StoryCourseViewModel> coursesList { get; set; }
        public virtual List<CourseViewModel> courses { get; set; }
        public string sortorder { get; set; }
        public HttpPostedFileBase picFile { get; set; }

        public virtual List<CategoryCheckBoxModel> allCategoryList { get; set; }
        public virtual List<CourseCheckboxModel> allCourseList { get; set; }
    }

}