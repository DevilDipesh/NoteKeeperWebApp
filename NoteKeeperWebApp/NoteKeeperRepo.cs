using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteKeeperWebApp
{
    public class NoteKeeperRepo
    {
        private NoteKeeperEntities context;

        public NoteKeeperRepo()
        {
            context = new NoteKeeperEntities();
        }

        public NoteKeeperRepo(NoteKeeperEntities context)
        {
            this.context = context;
        }

        public bool validateUser(string emailID, string password)
        {
            bool log = false;
            try
            {
                var user = context.UserAccesses.Where(s => s.EmailID == emailID && s.Password == password).ToList();

                if (user != null && user.Count() != 0)
                {
                    log = true;
                }
            }
            catch (Exception ex)
            {
                log = false;
            }

            return log;
        }

        public bool emailIDExists(string emailID)
        {
            bool sts = false;
            try
            {
                var user = context.UserAccesses.Where(e=>e.EmailID == emailID).ToList();
                if(user.Count != 0 )
                {
                    sts = true;
                }
                
            }
            catch(Exception ex)
            {
                sts = false;
            }
            return sts;
        }

        public bool addUser(UserAccess model)
        {
            bool updated = false;
            try
            {
                context.UserAccesses.Add(model);
                context.SaveChanges();
                updated = true;
            }
            catch (Exception ex)
            {
                updated = false;
            }

            return updated;
        }

        public bool userRegistration(UserDetail model, string emailID)
        {
            bool log = false;
            try
            {
                var id = context.UserAccesses.Where(a => a.EmailID == emailID).ToList();
                if (id != null && id.Count != 0)
                {
                    model.ID = id[0].ID;
                }
                model.PostDate = DateTime.Now;
                context.UserDetails.Add(model);
                context.SaveChanges();
                log = true;
            }
            catch (Exception ex)
            {
                log = false;
            }

            return log;
        }

        public bool userRegistration(UserDetail model)
        {
            bool log = false;
            try
            {
                UserDetail entity = context.UserDetails.FirstOrDefault(e => e.UserID == model.UserID);
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.BirthDate = model.BirthDate;
                entity.PostDate = DateTime.Now;
                context.SaveChanges();
                log = true;
            }
            catch (Exception ex)
            {
                log = false;
            }

            return log;
        }

        public bool registeredUser(string fname, string lname)
        {
            bool sts = false;
            try
            {
                var user = context.UserDetails.Where(a => a.FirstName == fname && a.LastName == lname).ToList();
                if (user != null && user.Count() != 0)
                {
                    sts = true;
                }
            }
            catch (Exception ex)
            {
                sts = false;
            }
            return sts;
        }

        public bool searchUser(string emailID)
        {
            bool sts = false;
            try
            {
                var user = (from a in context.UserAccesses join b in context.UserDetails on a.ID equals b.ID where a.EmailID == emailID select b.ID).ToList();
                if (user != null && user.Count() != 0)
                {
                    sts = true;
                }
            }
            catch (Exception ex)
            {
                sts = false;
            }
            return sts;
        }

        public List<UserDetail> getUser(string emailID)
        {
            var user = (from a in context.UserDetails join b in context.UserAccesses on a.ID equals b.ID where b.EmailID == emailID select a).ToList();
            return user;

        }
        public UserDetail getUserbyID(int? userID)
        {
            var user = context.UserDetails.FirstOrDefault(e => e.UserID == userID);
            return user;
        }



        public bool addNotes(Note model, string emailID)
        {
            bool sts = false;
            try
            {
                var user = context.UserAccesses.FirstOrDefault(e => e.EmailID == emailID);
                var note = context.UserDetails.FirstOrDefault(e => e.ID == user.ID);
                model.userID = note.UserID;
                model.PostDate = DateTime.Now;
                context.Notes.Add(model);
                context.SaveChanges();
                sts = true;
            }
            catch (Exception ex)
            {
                sts = false;
            }
            return sts;
        }

        public List<Note> listNotes()
        {
            return context.Notes.ToList();
        }

        public Note getNoteByID(int ID)
        {
            return context.Notes.Where(e => e.NotesID == ID).FirstOrDefault();
        }

        public bool updateNotes(Note model)
        {
            bool sts = false;

            try
            {
                var entity = context.Notes.FirstOrDefault(e => e.NotesID == model.NotesID);
                entity.Note1 = model.Note1;
                if(model.share!=null)
                {
                    entity.share = model.share;
                }         
                entity.PostDate = DateTime.Now;
                context.SaveChanges();
                sts = true;
            }
            catch (Exception ex)
            {
                sts = false;
            }

            return sts;
        }

        public UserAccess userAccessByID(int ID)
        {
            return context.UserAccesses.FirstOrDefault(e => e.ID == ID);
        }

        public bool deleteNote(Note entity)
        {
            bool sts = false;
            try
            {
                context.Notes.Remove(entity);
                context.SaveChanges();
                sts = true;
            }
            catch (Exception)
            {
                sts = false;
            }
            return sts;
        }

        public bool deleteAccount(UserDetail entity)
        {
            bool sts = false;
            try
            {
                UserAccess user = context.UserAccesses.FirstOrDefault(e => e.ID == entity.ID);
                var note = context.Notes.Where(e => e.userID == entity.UserID).ToList();
                foreach (var obj in note)
                {
                    context.Notes.Remove(obj);
                }
                context.UserDetails.Remove(entity);
                context.UserAccesses.Remove(user);
                context.SaveChanges();
                sts = true;
            }
            catch (Exception)
            {
                sts = false;
            }
            return sts;
        }
    }
}