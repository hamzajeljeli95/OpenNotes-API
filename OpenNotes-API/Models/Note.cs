using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenNotes_API.Models
{
    public class Note
    {
        public String UUID { get; set; }
        [Required]
        public String Content { get; set; }
        [Required]
        public String LastEditTimeDate { get; set; }

        public Note()
        {
            UUID = Guid.NewGuid().ToString();
        }
    }
}
