using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFKviz
{
    public class Szavak
    {
        public int Id { get; set; }
        public string Szo { get; set; }
        public string Jelentese { get; set; }
        public bool Tanult { get; set; }
        public int HelyesValaszokSzama { get; set; }
    }
}
