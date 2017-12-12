using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        public string Description { get; set; }

        public ApplicationUser User { get; set; }

        public string UserName { get; set; }
        public virtual Annoucement MyAnnoucement { get; set; }
    }
}
