using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteKeeperWebApp.Models
{
    public class ViewNotes
    {
        public int NotesID { get; set; }

        [Display(Name = "Note")]
        public string Note1 { get; set; }

        public string Name { get; set; }
        public Nullable<int> userID { get; set; }

        [Display(Name = "Shared With")]
        public string shared_With { get; set; }
        public string PostedBy { get; set; }

        [Display(Name = "Post Date")]
        public Nullable<System.DateTime> PostDate { get; set; }
    }
}