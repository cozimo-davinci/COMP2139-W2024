using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace COMP2139_Lab1.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int ProjectCommentID { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The comment should not exceed 500 characters!")]
        public string? Content {  get; set; }

        [Display(Name = "Posted Date")]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }
        public int projectID { get; set; }

        public Project? Project { get; set; }
    }
}
