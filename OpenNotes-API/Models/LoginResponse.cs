using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNotes_API.Models
{
    class LoginResponse
    {
        public String UserUUID { get; set; }
        public Boolean CanConnect { get; set; }
    }
}
