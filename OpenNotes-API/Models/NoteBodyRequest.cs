using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNotes_API.Models
{
    class NoteBodyRequest
    {
        [Required]
        public String userUUID { get; set; }
        [Required]
        public Note note { get; set; }
    }
}
