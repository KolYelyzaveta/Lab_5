using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna2
{
    public class Hotel
    {
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
        public double Cash { get; set; }
        public bool IsOpen { get; set; }

        public Hotel(string name)
        {
            this.Name = name;
            this.Rooms = new List<Room>();
            this.Cash = 0;
        }

        public Hotel() { }
    }
}
