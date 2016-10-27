using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Boat : Vehicle
    {
        public int Speed { get; private set; }
        public int NumberOfBeds { get; private set; }

        public Boat(int iNumberOfWheels, string iColor, string iRegNumber, int iSpeed, int iNumberOfBeds)
        {
            NumberOfWheels = iNumberOfWheels;
            Color = iColor;
            RegNumber = iRegNumber;
            Speed = iSpeed;
            NumberOfBeds = iNumberOfBeds;
        }

        public override string ToString()
        {
            return String.Format("Boat, {0}, {1}, {2}, {3}, {4}", NumberOfWheels, Color, RegNumber, Speed, NumberOfBeds);
        }
    }
}
