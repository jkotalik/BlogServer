using System;
using System.ComponentModel.DataAnnotations;

namespace BlogServer.Models
{
    public class BlogPost
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The Title must be less than 50 characters")]
        public string Title { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Last Edited")]
        [DataType(DataType.DateTime)]
        public DateTime EditedDate { get; set; }
        [Required]
        [StringLength(10000, ErrorMessage = "The body must be less than 10000 characters", MinimumLength = 1)]
        public string Body { get; set; }

        [Display(Name = "Created By")]
        public string NameIdentifier { get; set; }

        [Display(Name = "Views")]
        public int VisitCount { get; set; }
        public bool EditAndDeletePermissions { get; set; }
        public string LoginName { get; set; }
    }
}
