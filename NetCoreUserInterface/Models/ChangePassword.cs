using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreUserInterface.Models
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }
}
