using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteKeeperWebApp.Models
{
    public class Notes
    {
        public string Note { get; set; }

        [Display(Name = "Share With")]
        public string share { get; set; }
    }
}