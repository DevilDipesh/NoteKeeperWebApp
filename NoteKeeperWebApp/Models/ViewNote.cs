using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteKeeperWebApp.Models
{
    public class ViewNote
    {
        public int NotesID { get; set; }

        [Display(Name = "Note")]
        public string Note1 { get; set; }

        [Display(Name = "Shared With")]
        public String shared_With { get; set; }

        [Display(Name = "posted By")]
        public String PostedBy { get; set; }

        [Display(Name = "Post Date")]
        public Nullable<System.DateTime> PostDate { get; set; }
    }
}