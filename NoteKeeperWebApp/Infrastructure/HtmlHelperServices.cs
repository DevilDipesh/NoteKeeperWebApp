using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteKeeperWebApp.Infrastructure
{
    public static class HtmlHelperServices
    {
        public static List<SelectListItem> shareNote(this HtmlHelper html, string emailID)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (NoteKeeperEntities context = new NoteKeeperEntities())
            {
                var users = context.UserDetails.ToList();

                objList.Add(new SelectListItem { Text="Private with me",Value="Private with me"});

                foreach(var user in users)
                {
                    UserAccess obj = context.UserAccesses.FirstOrDefault(e => e.ID == user.ID);
                    if(obj.EmailID != emailID)
                    {
                        objList.Add(new SelectListItem { Text = user.FirstName + " " + user.LastName, Value = user.ID.ToString() });
                    }
                    
                        
                }
                
            }

            return objList;      
        }
    }
}