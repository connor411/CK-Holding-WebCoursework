using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    /// <summary>
    /// Annoucement Model
    /// </summary>
    public class Annoucement
    {
        // The key for an annoucement
        [Key]
        public int Id { get; set; }

        // The user who posted the 
        public virtual ApplicationUser User { get; set; }

        // The username of the user who posted the annoucement
        public string UserName { get; set; }

        // The title of an annoucement
        [Required]
        [StringLength(180)]
        public string Title { get; set; }

        // The description of an annoucement
        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        // The date and time when the annoucement was posted
        public DateTime DateAndTimeOfPost { get; set; }

        // The view counter an annoucement
        public int counter { get; set; }

        // The image location of an annoucement
        public string ImageLocation { get; set; }

        // The image name of an image used in an annoucement
        public string ImageName { get; set; }
    }
}
