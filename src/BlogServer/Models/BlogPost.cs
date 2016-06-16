using System;
using System.ComponentModel.DataAnnotations;

namespace BlogServer.Models
{
    public class BlogPost
    {
        private DateTime Creation = DateTime.MinValue;
        public int ID { get; set; }
        public string Title { get; set; }
        [Display(Name = "Post Date")]
        [DataType(DataType.DateTime)]
        // TODO move this logic to the controller, 
        // From what I remember you mentioned that models should just be data objects
        public DateTime Date {
            get { return (Creation == DateTime.MinValue) ? DateTime.Now : Creation; }
            set { Creation = value; }
        }
        public string Body { get; set; }
        [Display(Name = "Actions")]
        public string Options { get; set; }
        public string Owner { get; set; }
        public int VisitCount { get; set; }
        public bool Edited { get; set; }
        public string Creator { get; set; }
    }
}
