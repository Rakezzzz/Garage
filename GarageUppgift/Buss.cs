using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Buss : Vehicle
    {
        public int Seats { get; private set; }
        public string Route { get; private set; }
        public Buss(int iNumberOfWheels, string iColor, string iRegNumber, int iSeats, string iRoute)
        {
            NumberOfWheels = iNumberOfWheels;
            Color = iColor;
            RegNumber = iRegNumber;
            Seats = iSeats;
            Route = iRoute;

        }

        public override string ToString()
        {
            return String.Format("Buss, {0}, {1}, {2}, {3}, {4}", NumberOfWheels, Color, RegNumber, Seats, Route);
        }
    }
}
