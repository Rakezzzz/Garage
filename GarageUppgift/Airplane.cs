using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Airplane:Vehicle
    {
        public int NumberOfWings { get; private set; }
        public bool Pontoons { get; private set; }
        public Airplane(int iNumberOfWheels, string iColor, string iRegNumber, int iNumberOfWings, bool iPontoons)
        {
            NumberOfWheels = iNumberOfWheels;
            Color = iColor;
            RegNumber = iRegNumber;
            NumberOfWings = iNumberOfWings;
            Pontoons = iPontoons;
        }

        public override string ToString()
        {
            return String.Format("Airplane, {0}, {1}, {2}, {3}, {4}", NumberOfWheels, Color, RegNumber,NumberOfWings, Pontoons);
        }
    }
}
