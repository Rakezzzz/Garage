using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Motorcycle : Vehicle
    {
        public string Name { get; private set; }
        public int Year { get; private set; }

        public Motorcycle(int iNumberOfWheels, string iColor, string iRegNumber, string iName, int iYear)
        {
            NumberOfWheels = iNumberOfWheels;
            Color = iColor;
            RegNumber = iRegNumber;
            Name = iName;
            Year = iYear;
        }

        public override string ToString()
        {
            return String.Format("Motorcycle, {0}, {1}, {2}, {3}, {4}", NumberOfWheels, Color, RegNumber, Name, Year);
        }
    }
}
