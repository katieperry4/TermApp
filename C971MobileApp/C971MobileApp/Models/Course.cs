using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp.Models
{
    [Table("course")]
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        [Column("courseId")]
        public int CourseID { get; set; }

        [Column("termId")]
        public int TermId { get; set; }

        [Column("courseName")]
        public string CourseName { get; set; }

        [Column("courseStatus")]
        public bool CourseStatus { get; set; }

        [Column("courseStart")]
        public DateTime CourseStart {  get; set; }

        [Column("courseEnd")]
        public DateTime CourseEnd { get; set; }

        public string CIName { get; set; }
        public string CIEmail { get; set; }
        public string CIPhone { get; set; }

        [Column("courseNotes")]
        public string CourseNotes { get; set; }

        [Column("startNotif")]
        public bool StartNotif { get; set; }

        [Column("endNotif")]
        public bool EndNotif { get; set; }

        public string FormattedCourseStart => CourseStart.ToString("MMM dd yyyy");
        public string FormattedCourseEnd => CourseEnd.ToString("MMM dd yyyy");
    }
}
