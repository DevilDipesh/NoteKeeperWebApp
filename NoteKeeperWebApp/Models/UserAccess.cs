using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteKeeperWebApp.Models
{
    public class UserAccess
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Email ID")]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}