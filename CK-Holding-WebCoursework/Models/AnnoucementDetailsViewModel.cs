using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    public class AnnoucementDetailsViewModel
    {
        public Annoucement Annoucement { get; set; }
        public List<Comment> Comments { get; set; }
        public int AnnoucementID { get; set; }

        public string Description { get; set; }

        //public ApplicationUser User { get; set; }
        public string CommentUser { get; set; }
        public int counter { get; set; }
    }
}
