using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    abstract class Vehicle:IEquatable<Vehicle>
    {
        public string RegNumber { get; protected set; }
        public string Color { get; protected set; }
        public int NumberOfWheels { get; protected set; }




        public bool Equals(Vehicle other)
        {
            if (this.RegNumber.ToLower() == other.RegNumber.ToLower())
                return true;
            else
                return false;
        }
    }
}
