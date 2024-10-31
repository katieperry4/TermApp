using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp.Models
{
    [Table("assessment")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        [Column("assessmentId")]
        public int AssessmentId { get; set; }

        [Column("courseId")]
        public int CourseId { get; set; }

        [Column("assessmentName")]
        public string AssessmentName { get; set; }

        [Column("assessmentStart")]
        public DateTime AssessmentStart {  get; set; }

        [Column("assessmentEnd")]
        public DateTime AssessmentEnd { get; set; }

        [Column("assessmentType")]
        public string AssessmentType { get; set; }

        [Column("startNotif")]
        public bool StartNotif {  get; set; }

        [Column("endNotif")]
        public bool EndNotif { get; set; }


        public string FormattedAssessmentStart => AssessmentStart.ToString("MMM dd yyyy");
        public string FormattedAssessmentEnd => AssessmentEnd.ToString("MMM dd yyyy");
    }
}
