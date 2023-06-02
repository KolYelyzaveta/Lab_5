using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna2
{
    public class Room
    {
        public int Places { get; set; }
        public int TakenPlaces { get; set; }
        public double Price { get; set; }
        public Status CurrentStatus { get; set; }

        public enum Status
        {
            FREE,
            LOCKED
        }

        public Room(int places, int takenPlaces, double price, Status status)
        {
            this.Places = places;
            this.TakenPlaces = takenPlaces;
            this.Price = price;
            this.CurrentStatus = status;
        }

        public Room() { }
    }
}
