using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    public class Annoucement
    {

        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateAndTimeOfPost { get; set; }

        public int counter { get; set; }


    }
}
