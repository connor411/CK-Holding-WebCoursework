using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CK_Holding_WebCoursework.Models
{
    /// <summary>
    /// A viewModel for an annoucement
    /// </summary>
    public class AnnoucementDetailsViewModel
    {
        // The annoucment used in the view model
        public Annoucement Annoucement { get; set; }

        // The list of comments for that annoucement
        public List<Comment> Comments { get; set; }

        // The annoucement Id of an annoucement
        public int AnnoucementID { get; set; }

        // The description of an annoucement
        public string Description { get; set; }

        // The user who made a new comment
        public string CommentUser { get; set; }

        // The view counter fo an annoucement
        public int counter { get; set; }
    }
}
