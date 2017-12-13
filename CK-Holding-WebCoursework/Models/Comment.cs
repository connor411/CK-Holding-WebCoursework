using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    /// <summary>
    /// A Comment Model
    /// </summary>
    public class Comment
    {
        // The key for a comment
        public int Id { get; set; }

        // The description of a comment
        public string Description { get; set; }

        // The user who posed the comment
        public ApplicationUser User { get; set; }

        // The username of the user who posted the comment
        public string UserName { get; set; }

        //The annoucement the comment is in
        public virtual Annoucement MyAnnoucement { get; set; }
    }
}
