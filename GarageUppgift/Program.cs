using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageUppgift
{
    class Program
    {
        static List<Dictionary<string, Action<string>>> Menues = new List<Dictionary<string, Action<string>>>();
        static int menuIndex = 0;
        //Garage<Vehicle> garage = new Garage<Vehicle>(20);
        static int index = 0;
        #region unmanaged
 
        /// <summary>
        /// This function sets the handler for kill events.
        /// </summary>
        /// <param name="Handler"></param>
        /// <param name="Add"></param>
        /// <returns></returns>
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
 
        //delegate type to be used of the handler routine
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);
 
        // control messages
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
 
        
        #endregion

        public static void dotdotdot()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
        }
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType, Garage<Vehicle> gar)
        {
            Console.Write("Saving");
            gar.SaveVehicles();
            dotdotdot();

            return true;
        }

        static void OnProcessExit(object sender, EventArgs e, Garage<Vehicle> gar)
        {
            Console.Write("Saving");
            gar.SaveVehicles();
            dotdotdot();
        }





        #region Henry

        static void Menu()
        {
            if (index >= Menues[menuIndex].Count()) { index = 0; }
            Console.Clear();
            int i = 0;
            string active = "";
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                i = 0;
                active = "";
                foreach (KeyValuePair<string, Action<string>> alternativ in Menues[menuIndex])
                {

                    Console.WriteLine();
                    //Console.CursorLeft = Indrag;
                    if (index == i)
                    {
                        HighLight(alternativ.Key);
                        active = alternativ.Key;
                    }
                    else
                        Console.WriteLine(" " + alternativ.Key);
                    i += 1;
                }

                //Console.WriteLine("\n" + Msg + "\n");
                Console.WriteLine("Arrowkeys: " + (char)0x18 + (char)0x19 + (char)0x1A + (char)0x1B);
                Console.WriteLine("Enter: Interact" + (char)13);
                Console.WriteLine("Escape: Back\n\n\n");

                if (Navigation(active)) { return; }
            }
        }

        static bool Navigation(string active)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
                if (index == 0)
                    index = Menues[menuIndex].Count() - 1;
                else
                    index -= 1;
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                if (index == Menues[menuIndex].Count() - 1)
                    index = 0;
                else
                    index += 1;
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (active == "Backa")
                {
                    index = 0;
                    return true;
                }

                Menues[menuIndex][active].Invoke(active);
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                menuIndex = 0;
                index = 0;
                Console.Clear();
            }
                

            return false;
        }

        static void HighLight(string alternativ)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" " + alternativ);
            Console.ResetColor();
            Console.WriteLine();
        }

        static void AddToDictionary(Garage<Vehicle> gar)
        {

            Menues.Add(new Dictionary<string, Action<string>>());
            Menues.Add(new Dictionary<string, Action<string>>());
            Menues.Add(new Dictionary<string, Action<string>>());
            Menues.Add(new Dictionary<string, Action<string>>());
            Menues.Add(new Dictionary<string, Action<string>>());


            Menues[0].Add("Show all vehicles.", (x) => { 
                Console.Clear(); 
                Console.WriteLine(gar.ListAllVehicles());
                Console.ReadKey(); 
                Console.Clear(); });

            Menues[0].Add("Show all vehicle types.", (x) => { 
                Console.Clear(); 
                Console.WriteLine(gar.ListAllTypes()); 
                Console.ReadKey(); 
                Console.Clear(); });

            Menues[0].Add("Add vehicle.", (x) => { Console.Clear(); 
                menuIndex = 1; 
                index = 0; 
                Console.WriteLine("What kind of vehicle would you like to add? \n "); 
            
            });


            #region Remove Vehicle


            Menues[0].Add("Remove vehicle.", (x) => {


                string allvehicles = gar.ListAllVehicles();
                if (allvehicles != "")
                {
                    Menues[2] = new Dictionary<string, Action<string>>();
                    foreach (var item in allvehicles.Split('\n'))
                    {
                        string thisVehicle;
                        if (item != "")
                        {
                            thisVehicle = item.Split('=')[1].Trim();
                            Menues[2].Add("Vehicle: " + thisVehicle, (y) => { Console.Clear(); gar.RemoveVehicle(thisVehicle); menuIndex = 0; index = 0; Console.WriteLine("Your vehicle was removed!"); Console.ReadKey(); });

                        }


                    }


                    menuIndex = 2;
                    index = 0;
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("There is no vehicles in the garage!");
                    Console.ReadKey();
                    Console.Clear();
                }
            
            
            });

            #endregion


            Menues[0].Add("Search for vehicle", (x) => {

                menuIndex = 4;
                index = 0;
                Console.Clear();

                Menues[4].Add("Search by RegNum", (y) => {
                    Console.WriteLine("What RegNum would you like to search on?");
                    
                    string search = gar.SearchByRegNum(Console.ReadLine());

                    if (search != "")
                        Console.WriteLine(search);
                    else
                        Console.WriteLine("There is no vehicle with that reg num in the garage.");

                    Console.ReadKey();
                    Console.Clear();
                    index = 0;

                });

                Menues[4].Add("Search by color", (y) =>
                {
                    Console.WriteLine("What Color would you like to search on?");

                    string search = gar.SearchByColor(Console.ReadLine());

                    if (search != "")
                        Console.WriteLine(search);
                    else
                        Console.WriteLine("There is no vehicle with that color num in the garage.");

                    Console.ReadKey();
                    Console.Clear();

                });



                Menues[4].Add("Search by number of wheels", (y) =>
                {
                    Console.WriteLine("What number of wheels would you like to search on?");
                    string search;
                    try
                    {
                        search = gar.SearchByNumberOfWheels(int.Parse(Console.ReadLine()));
                    }
                    catch (Exception)
                    {

                        search = "";
                    }
                    

                    if (search != "")
                        Console.WriteLine(search);
                    else
                        Console.WriteLine("There is no vehicle with that number of wheels in the garage.");

                    Console.ReadKey();
                    Console.Clear();

                });






            });


            #region Create Garage

            Menues[0].Add("Create new Garage:", (x) => {
                string FileName = @"C:\Users\elev\Documents\Visual Studio 2013\Garages.txt";


                string[] Garages = System.IO.File.ReadAllLines(FileName);
                bool Continue = true;
                
                Console.Clear();
                do
                {
                    Console.WriteLine("What name do you want to have on the new garage?");
                    string newgarage = Console.ReadLine();
                    Continue = true;
                    int spots = 0;
                    do
                    {
                        try
                        {
                            Console.WriteLine("How many spots do you want in your garage?");
                            spots = int.Parse(Console.ReadLine());
                            if(spots>0)
                                Continue = false;
                            else
                            {
                                Console.WriteLine("You can not have a garage with no spots!");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Wrong input! Try again!");
                        }

                    } while (Continue);

                    if (!Garages.Contains(newgarage))
                    {
                        Continue = false;
                        Console.Clear();
                        FileStream appendWrite = File.Open(FileName, FileMode.Append);
                        StreamWriter write = new StreamWriter(appendWrite);
                        write.WriteLine(newgarage + ":" + spots);
                        write.Close();
                        appendWrite.Close();
                        FileStream temp = new FileStream(@"C:\Users\elev\Documents\Visual Studio 2013\" + newgarage + ".txt", FileMode.Create);
                        //File.Create(@"C:\Users\elev\Documents\Visual Studio 2013\" + newgarage + ".txt");
                        temp.Close();

                        Console.WriteLine("You need to restart your application before your new garage is avalible.\nIt is currently in construction!");
                        Console.ReadKey();

                        
                    }
                    else
                    {
                        Console.WriteLine("It allready exists. Try again.");
                        Console.ReadKey();
                        Console.Clear();
                    }


                } while (Continue);



                Console.Clear();

            
            
            });
            #endregion



            #region Load Garage
            Menues[0].Add("Load Garage", (x) => {

                string FileName = @"C:\Users\elev\Documents\Visual Studio 2013\Garages.txt";
                string[] Garages = System.IO.File.ReadAllLines(FileName);
                Menues[3] = new Dictionary<string, Action<string>>();

                foreach (var item in Garages)
                {
                    string garageName = item.Split(':')[0];
                    int garageSpots = int.Parse(item.Split(':')[1]);


                    if (item != "" && garageName != gar.GarageName)
                    {
                        Menues[3].Add("Garage: " + garageName, (y) =>
                        {
                            Console.Clear();
                            menuIndex = 0;
                            index = 0;
                            gar.SaveVehicles();
                            gar.removeAllVehicles();
                            LoadVehicles(gar, @"C:\Users\elev\Documents\Visual Studio 2013\" + garageName + ".txt");

                            gar.changeMaxParkingSpots(garageSpots);
                            gar.GarageName = garageName;
                            
                            
                            Console.Write("Loading");
                            dotdotdot();
                            Console.Clear();

                        });
                    }
                }

                menuIndex = 3;
                index = 0;
                Console.Clear();
                Console.WriteLine("Coose what garage to load. \n");



            });


            #endregion


            Menues[0].Add("Exit application\n\n", (x) => { Environment.Exit(0); });



            
            #region Menu Car


            Menues[1].Add("Car", (x) => { 
                
                int NumberOfWheels = 0; 
                string Color;
                string RegNumber; 
                int NumberOfWindows = 0;
                bool SunRoof;

                Console.Clear();

                    bool input = true;
                    do
                    {
                        try
                        {
                            Console.WriteLine("Number of Wheels: ");
                            NumberOfWheels = int.Parse(Console.ReadLine());
                            input = false;
                        }
                        catch (Exception)
                        {

                            Console.Clear();
                            Console.WriteLine("Wrong input. Try again!");
                        }

                    } while (input);

                    Console.WriteLine("Color: ");
                    Color = Console.ReadLine();
                    Console.WriteLine("RegNum: ");
                    RegNumber = Console.ReadLine();



                    input = true;
                    do
                    {
                        try
                        {
                            Console.WriteLine("NumberOfWindows: ");
                            NumberOfWindows = int.Parse(Console.ReadLine());
                            input = false;
                        }
                        catch (Exception)
                        {

                            Console.Clear();
                            Console.WriteLine("Wrong input. Try again!");
                        }

                    } while (input);

                    Console.WriteLine("Does it have a sunroof? (y/n)");
                    string sunroof = Console.ReadLine();
                    if (sunroof.ToLower() == "y")
                        SunRoof = true;

                    else
                    {
                        SunRoof = false;
                        Console.WriteLine("no sunroof");
                    }


                    if(gar.AddVehicle(new Car(NumberOfWheels, Color, RegNumber, NumberOfWindows, SunRoof)))
                        Console.WriteLine("Your car has been added.");
                    else
                        Console.WriteLine("That Registration number allready exists!");

                Console.ReadKey();
                Console.WriteLine("Press the any key.");
                Console.Clear();
                menuIndex = 0;
                index = 0;



            
            
            });

#endregion

            #region Menu Airplane


            Menues[1].Add("Airplane", (x) => {

                int NumberOfWheels = 0;
                string Color;
                string RegNumber;
                int NumberOfWings = 0;
                bool Pontoons;

                Console.Clear();

                bool input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of Wheels: ");
                        NumberOfWheels = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Color: ");
                Color = Console.ReadLine();
                Console.WriteLine("RegNum: ");
                RegNumber = Console.ReadLine();



                input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("NumberOfWings: ");
                        NumberOfWings = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Does it have poontons? (y/n)");
                string pontoons = Console.ReadLine();
                if (pontoons.ToLower() == "y")
                    Pontoons = true;

                else
                {
                    Pontoons = false;
                    Console.WriteLine("No pontoons");
                }


               


                if (gar.AddVehicle(new Airplane(NumberOfWheels, Color, RegNumber, NumberOfWings, Pontoons)))
                {
                    Console.WriteLine("Your airplane has been added.");


                }
                else
                    Console.WriteLine("That Registration number allready exists!");

                Console.ReadKey();
                Console.WriteLine("Press the any key.");
                Console.Clear();
                menuIndex = 0;
                index = 0;
            
            
            });

#endregion

            #region Menu Motorcycle

            Menues[1].Add("Motorcycle", (x) => {



                int NumberOfWheels = 0;
                string Color;
                string RegNumber;
                string Name;
                int Year = 0;

                Console.Clear();


                Console.WriteLine("Name: ");
                Name = Console.ReadLine();

                bool input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of Wheels: ");
                        NumberOfWheels = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Color: ");
                Color = Console.ReadLine();
                Console.WriteLine("RegNum: ");
                RegNumber = Console.ReadLine();



                input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Year: ");
                        Year = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);



                if(gar.AddVehicle(new Motorcycle(NumberOfWheels, Color, RegNumber, Name, Year)))
                    Console.WriteLine("Your motorcycle has been added.");
                else
                    Console.WriteLine("That Registration number allready exists!");

                Console.ReadKey();
                Console.WriteLine("Press the any key.");
                Console.Clear();
                menuIndex = 0;
                index = 0;

            
            
            
            
            });


            #endregion

            #region Menu Buss

            Menues[1].Add("Buss", (x) => {


                int NumberOfWheels = 0;
                string Color;
                string RegNumber;
                int Seats = 0;
                string Route;

                Console.Clear();




                bool input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of Wheels: ");
                        NumberOfWheels = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Color: ");
                Color = Console.ReadLine();
                Console.WriteLine("RegNum: ");
                RegNumber = Console.ReadLine();



                input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of seats: ");
                        Seats = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Route: ");
                Route = Console.ReadLine();



                if(gar.AddVehicle(new Buss(NumberOfWheels, Color, RegNumber, Seats, Route)))
                    Console.WriteLine("Your buss has been added.");
                else
                    Console.WriteLine("That Registration number allready exists!");

                Console.ReadKey();
                Console.WriteLine("Press the any key.");
                Console.Clear();
                menuIndex = 0;
                index = 0;

            
            
            
            });

            #endregion

            #region Menu Boat

            Menues[1].Add("Boat\n\n\n", (x) => {


                int NumberOfWheels = 0;
                string Color;
                string RegNumber;
                int Speed = 0;
                int NumberOfBeds = 0;

                Console.Clear();

                bool input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of Wheels: ");
                        NumberOfWheels = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                Console.WriteLine("Color: ");
                Color = Console.ReadLine();
                Console.WriteLine("RegNum: ");
                RegNumber = Console.ReadLine();



                input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Speed: ");
                        Speed = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);

                input = true;
                do
                {
                    try
                    {
                        Console.WriteLine("Number of beds in the boat: ");
                        NumberOfBeds = int.Parse(Console.ReadLine());
                        input = false;
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                        Console.WriteLine("Wrong input. Try again!");
                    }

                } while (input);


                if(gar.AddVehicle(new Boat(NumberOfWheels, Color, RegNumber, Speed, NumberOfBeds)))
                    Console.WriteLine("Your boat has been added.");
                else
                    Console.WriteLine("That Registration number allready exists!");

                Console.ReadKey();
                Console.WriteLine("Press the any key.");
                Console.Clear();
                menuIndex = 0;
                index = 0;

            
            
            });

            #endregion

        }
        

        #endregion



















        static void Main(string[] args)
        {
            Garage<Vehicle> garage = new Garage<Vehicle>(20);
            HandlerRoutine hr = new HandlerRoutine(Control => ConsoleCtrlCheck(Control, garage));
            GC.KeepAlive(hr);
            SetConsoleCtrlHandler(hr, true);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender, e) => OnProcessExit(sender, e, garage));
            //string defaultgarage = @"C:\Users\elev\Documents\Visual Studio 2013\Garages.txt";

            LoadVehicles(garage, @"C:\Users\elev\Documents\Visual Studio 2013\" + garage.GarageName + ".txt");

            AddToDictionary(garage);
            Menu();





            Console.ReadKey();


        }

        static void LoadVehicles(Garage<Vehicle> garage, string whatFile)
        {
            string[] text = System.IO.File.ReadAllLines(whatFile);

            foreach (string line in text)
            {
                string[] tempArray = line.Split(',');

                switch (tempArray[0])
                {
                    case "Car":
                        garage.AddVehicle(new Car(int.Parse(tempArray[1]), tempArray[2].Trim(), tempArray[3].Trim(), int.Parse(tempArray[4]), bool.Parse(tempArray[5])));
                        break;
                    case "Airplane":
                        garage.AddVehicle(new Airplane(int.Parse(tempArray[1]), tempArray[2].Trim(), tempArray[3].Trim(), int.Parse(tempArray[4]), bool.Parse(tempArray[5])));
                        break;
                    case "Motorcycle":
                        garage.AddVehicle(new Motorcycle(int.Parse(tempArray[1]), tempArray[2].Trim(), tempArray[3].Trim(), tempArray[4].Trim(), int.Parse(tempArray[5])));
                        break;
                    case "Buss":
                        garage.AddVehicle(new Buss(int.Parse(tempArray[1]), tempArray[2].Trim(), tempArray[3].Trim(), int.Parse(tempArray[4]), tempArray[5].Trim()));
                        break;
                    case "Boat":
                        garage.AddVehicle(new Boat(int.Parse(tempArray[1]), tempArray[2].Trim(), tempArray[3].Trim(), int.Parse(tempArray[4]), int.Parse(tempArray[5])));
                        break;
                    default:
                        throw new NotImplementedException();
                        
                }

            }
        }

    }
}
