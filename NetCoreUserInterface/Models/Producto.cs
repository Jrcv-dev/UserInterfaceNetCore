using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreUserInterface.Models
{
    public class Producto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string image { get; set; }
    }
}
