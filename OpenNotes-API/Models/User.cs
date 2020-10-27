using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenNotes_API.Models
{
    public class User
    {
        public String UUID { get; set; }
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
        public List<Note> Notes { get; }

        public User()
        {
            UUID = Guid.NewGuid().ToString();
            Notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            note.LastEditTimeDate = new DateTime().ToString();
            this.Notes.Add(note);
        }

        public void EditNote(Note note)
        {
            foreach(Note CurrNote in Notes)
            {
                if(CurrNote.UUID.Equals(note.UUID))
                {
                    CurrNote.Content = note.Content;
                    CurrNote.LastEditTimeDate = new DateTime().ToString();
                    break;
                }
            }
        }

        public void RemoveNote(String NoteUUID)
        {
            foreach (Note CurrNote in Notes)
            {
                if (CurrNote.UUID.Equals(NoteUUID))
                {
                    Notes.Remove(CurrNote);
                    break;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
