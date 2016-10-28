using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Car : Vehicle
    {
        public int NumberOfWindows { get; private set; }
        public bool SunRoof { get; private set; }   

        public Car(int iNumberOfWheels, string iColor, string iRegNumber, int iNumberOfWindows, bool iSunRoof)
        {
            NumberOfWheels = iNumberOfWheels;
            Color = iColor;
            RegNumber = iRegNumber;
            NumberOfWindows = iNumberOfWindows;
            SunRoof = iSunRoof;
        }

        public override string ToString()
        {
            return String.Format("Car, {0}, {1}, {2}, {3}, {4}", NumberOfWheels, Color, RegNumber, NumberOfWindows, SunRoof);
        }

    }
}
