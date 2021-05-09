using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace NoteKeeperWebApp.Controllers
{
    public class HomeController : Controller
    {
        NoteKeeperRepo repo = new NoteKeeperRepo();
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult saveUserDetails()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult saveUserDetails(Models.UserDetails model)
        {
            UserDetail entity = new UserDetail();
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.BirthDate = model.BirthDate;
            string emailID = (string)Session["User"];
            bool log = repo.userRegistration(entity, emailID);
            if (log)
            {
                return View("Success");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult ViewProfile()
        {
            if (Session["User"] != null)
            {
                var emailID = (string)Session["User"];
                var entity = repo.getUser(emailID);
                UserDetail model = new UserDetail();

                foreach (var a in entity)
                {
                    model.UserID = a.UserID;
                    model.FirstName = a.FirstName;
                    model.LastName = a.LastName;
                    model.BirthDate = a.BirthDate;
                    model.ID = a.ID;
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult EditProfile(int id)
        {
            var user = repo.getUserbyID(id);

            UserDetail model = new UserDetail();

            model.UserID = user.UserID;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.BirthDate = user.BirthDate;

            return View(model);

        }

        [HttpPost]
        public ActionResult EditProfile(UserDetail model)
        {
            var user = repo.getUserbyID(model.UserID);
            model.ID = user.ID;
            var status = repo.userRegistration(model);
            if(status)
            {
                return RedirectToAction("ViewProfile", "Home");

            }
            else
            {
                return View("Error");
            }

        }

        public ActionResult AddNote()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult AddNote(Models.Notes model)
        {
            string emailID = (string)Session["User"];
            Note entity = new Note();
            entity.Note1 = model.Note;
            if(model.share.Contains("Private with me"))
            {
                entity.share = model.share;
            }
            else
            {
                var shared = repo.userAccessByID(Convert.ToInt32(model.share));
                entity.share = shared.EmailID;
            }

            entity.PostedBy = emailID;
            bool status = repo.addNotes(entity, emailID);
            if (status)
            {
                return RedirectToAction("GetNotes", "Home");
            }
            else
            {
                ViewBag.NoteMsg = "Oops Something went wrong Please try again later";
                return View(model);
            }
        }

        public ActionResult GetNotes()
        {
            if (Session["User"] != null)
            {
                var notes = repo.listNotes();

                List<Models.ViewNotes> model = new List<Models.ViewNotes>();

                foreach (var entity in notes)
                {
                    UserDetail user = repo.getUserbyID(entity.userID);
                    Models.ViewNotes temp = new Models.ViewNotes
                    {
                        NotesID = entity.NotesID,
                        Note1 = entity.Note1,
                        Name = user.FirstName + " " + user.LastName,
                        shared_With = entity.share,
                        PostedBy = entity.PostedBy,
                        userID = entity.userID,
                        PostDate = entity.PostDate
                    };
                    model.Add(temp);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult GetNotesByID(int ID)
        {
            Note note = repo.getNoteByID(ID);

            Models.ViewNote model = new Models.ViewNote();

            model.NotesID = note.NotesID;
            model.Note1 = note.Note1;
            model.shared_With = note.share;
            model.PostedBy = note.PostedBy;
            model.PostDate = note.PostDate;

            return View(model);
        }

        [HttpGet]
        public ActionResult updateNote(int ID)
        {
            Note note = repo.getNoteByID(ID);

            Note entity = new Note();

            entity.NotesID = note.NotesID;
            entity.Note1 = note.Note1;
            entity.PostedBy = note.PostedBy;
            entity.share = note.share;
            entity.userID = note.userID;
            entity.PostDate = note.PostDate;

            return View("updateNote", entity);
        }

        [HttpPost]
        public ActionResult updateNote(Note model)
        {
            Note entity = new Note();

            entity.NotesID = model.NotesID;
            entity.Note1 = model.Note1;
            if(model.share != null)
            {
                if (model.share.Contains("Private with me"))
                {
                    entity.share = model.share;
                }
                else
                {
                    var shared = repo.userAccessByID(Convert.ToInt32(model.share));
                    entity.share = shared.EmailID;
                }
            }
            
            bool sts = repo.updateNotes(entity);
            if (sts)
            {
                return RedirectToAction("GetNotes","Home");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteNote(int ID)
        {
            Note model = repo.getNoteByID(ID);
            return View(model);
        }

        public ActionResult DeleteNoteConfirm(Note model)
        {
            Note note = repo.getNoteByID(model.NotesID);

            var status = repo.deleteNote(note);
            if(status)
            {
                return View("DeleteSuccess");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteAccount(int ID)
        {
            UserDetail model = repo.getUserbyID(ID);
            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteAccountConfirm(UserDetail model)
        {
            UserDetail user = repo.getUserbyID(model.UserID);

            var status = repo.deleteAccount(user);
            if (status)
            {
                Session["User"] = null;
                return View("AccountDeleted");
            }
            else
            {
                return View("Error");
            }
        }
    }
}