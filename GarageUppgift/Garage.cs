using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageUppgift
{


    class Garage<T> :IEnumerable<T>
        where T : Vehicle
    {
        private List<T> vehicles = new List<T>();
        public int MaxParkingSpots { get; private set; }
        public string GarageName { get; set; }



        public Garage(int iMaxParkingSpots = 20, string iGarageName = "Garaget")
        {
            MaxParkingSpots = iMaxParkingSpots;
            GarageName = iGarageName;

        }
        public void changeMaxParkingSpots(int iMax)
        {
            MaxParkingSpots = iMax;
        }
        public void removeAllVehicles()
        {
            vehicles = new List<T>();
        }
        /// <summary>
        /// Saves the vehicles to the file C:\Users\elev\Documents\Visual Studio 2013\Garaget.txt
        /// </summary>
        public void SaveVehicles()
        {
            string[] save = new string[vehicles.Count];
            for (int i = 0; i < vehicles.Count; i++)
            {
                save[i] = vehicles[i].ToString();
            }
            System.IO.File.WriteAllLines(@"C:\Users\elev\Documents\Visual Studio 2013\" + GarageName + ".txt", save);
        }



        /// <summary>
        /// Adds a vehicle to the internal storage as long as there is avalible spots and it does not exist allready. ( MaxparkingSpots is the limit )
        /// </summary>
        /// <param name="vehicle">The vehicle you want to add. </param>
        /// <returns>Returns true if the vehicle was added and false if it was not. </returns>

        public bool AddVehicle(T vehicle)
        {
            if (MaxParkingSpots > vehicles.Count && !vehicles.Contains(vehicle))
            {

                vehicles.Add(vehicle);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes a vehicle from the internal storage if it exists.
        /// </summary>
        /// <param name="iReg">The registration number of the vehicle you want to remove.</param>
        /// <returns>If the vehicle is removed it returns true. If the vehicle is not removed or doesnt exist it returns false.</returns>

        public bool RemoveVehicle(string iReg)
        {

            T[] ifVehicleExist = (from vehicle in vehicles
                                  where vehicle.RegNumber == iReg
                                  select vehicle).ToArray();

            try
            {
                if (ifVehicleExist[0] != null)
                {
                    vehicles.Remove(ifVehicleExist[0]);
                    return true;
                }
                else
                    return false;


            }
            catch (Exception)
            {

                return false;
            }


        }
        /// <summary>
        /// Search for all the vehicles that has a perticular color.
        /// </summary>
        /// <param name="iColor">The color you want the vehicles you are serching for to have.</param>
        /// <returns>Returns a string array with all the elements as string representation of the vehicles that matches the color.</returns>

        public string SearchByColor(string iColor)
        {
            return ListAllVehicles((from vehicle in vehicles where vehicle.Color == iColor select vehicle).ToList());
        }

        public string SearchByNumberOfWheels(int iNumberOfWheels)
        {
            return ListAllVehicles((from vehicle in vehicles where vehicle.NumberOfWheels == iNumberOfWheels select vehicle).ToList());
        }

        public string SearchByRegNum(string iRegNum)
        {
            return ListAllVehicles((from vehicle in vehicles where vehicle.RegNumber == iRegNum select vehicle).ToList());
        }





        public string ListAllVehicles()
        {
            return ListAllVehicles(vehicles);
        }



        public string ListAllVehicles(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                sb.AppendFormat("{0} with registration number = {1}\n", ParsedType(item), item.RegNumber);
            }


            return sb.ToString();
        }


        private string ParsedType(T item)
        {
            return item.GetType().ToString().Split('.')[1];
        }



        public string ListAllTypes()
        {
            Dictionary<string, int> tempGarage = new Dictionary<string, int>();
            StringBuilder sb = new StringBuilder();
            foreach (T item in vehicles)
            {
                string tempString = ParsedType(item);

                if (tempGarage.ContainsKey(tempString))
                {
                    tempGarage[tempString] += 1;
                }
                else
                    tempGarage.Add(tempString, 1);
            }
            foreach (KeyValuePair<string,int> item in tempGarage)
            {
                sb.AppendFormat("{1} number of {0}\n", item.Key, item.Value);
            }

            return sb.ToString();

        }


        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in vehicles)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
