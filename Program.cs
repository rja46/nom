using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodCS_Settlement_And_Household
{
    class Household
    {
        private static Random rnd = new Random();
        protected double chanceEatOutPerDay;
        protected int xCoord, yCoord, ID;
        protected static int nextID = 1;
        public int numTimesAteOut = 0;

        public Household(int x, int y)
        {
            xCoord = x;
            yCoord = y;
            chanceEatOutPerDay = rnd.NextDouble();
            ID = nextID;
            nextID++;
        }

        public string GetDetails()
        {
            string details;
            details = ID.ToString().PadRight(4, ' ');
            details += "     Coordinates: (" + xCoord.ToString().PadLeft(4, '0') + ", " + yCoord.ToString().PadLeft(4, '0') + ")     Eat out probability: " + chanceEatOutPerDay.ToString().Substring(0,5).PadRight(5) + "     Times eaten out: " + numTimesAteOut.ToString().PadLeft(5, '0');
            return details;
        }

        public double GetChanceEatOut()
        {
            return chanceEatOutPerDay;
        }

        public int GetX()
        {
            return xCoord;
        }

        public int GetY()
        {
            return yCoord;
        }
        public int GetNumTimesAteOut()
        {
            return numTimesAteOut;
        }
    }

    class Settlement
    {
        private static Random rnd = new Random();
        protected int startNoOfHouseholds, xSize, ySize;
        protected List<Household> households = new List<Household>();

        public Settlement()
        {
            xSize = 1000;
            ySize = 1000;
            startNoOfHouseholds = 250;
            CreateHouseholds();
        }

        public int GetNumberOfHouseholds()
        {
            return households.Count;
        }

        public int GetXSize()
        {
            return xSize;
        }

        public int GetYSize()
        {
            return ySize;
        }

        public void GetRandomLocation(ref int x, ref int y)
        {
            x = Convert.ToInt32(rnd.NextDouble() * xSize);
            y = Convert.ToInt32(rnd.NextDouble() * ySize);
        }

        public void CreateHouseholds()
        {
            for (int count = 0; count < startNoOfHouseholds; count++)
            {
                AddHousehold();
            }
        }

        public void AddHousehold()
        {
            int x = 0, y = 0;
            GetRandomLocation(ref x, ref y);
            Household temp = new Household(x, y);
            households.Add(temp);
        }

        public void DisplayHouseholds()
        {
            Console.WriteLine("\n**********************************");
            Console.WriteLine("*** Details of all households: ***");
            Console.WriteLine("**********************************\n");
            int i = 0;
            foreach (var h in households)
            {
                Console.WriteLine(h.GetDetails());
                i++;
                if (i % 20 == 0)
                {
                    Console.WriteLine("Enter to continue...");
                    Console.ReadLine();
                }
            }
            Console.WriteLine();
        }

        public bool FindOutIfHouseholdEatsOut(int householdNo, ref int x, ref int y)
        {
            double eatOutRNo = rnd.NextDouble();
            x = households[householdNo].GetX();
            y = households[householdNo].GetY();
            if (eatOutRNo < households[householdNo].GetChanceEatOut())
            {
                households[householdNo].numTimesAteOut++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class LargeSettlement : Settlement
    {
        public LargeSettlement(int extraXSize, int extraYSize, int extraHouseholds)
            : base()
        {
            xSize += extraXSize;
            ySize += extraYSize;
            startNoOfHouseholds += extraHouseholds;
            for (int count = 1; count < extraHouseholds + 1; count++)
            {
                AddHousehold();
            }
        }
    }

    class SmallSettlement : Settlement
    {
        public SmallSettlement(int newXSize, int newYSize, int newHouseholds)
            : base()
        {
            xSize = newXSize;
            ySize = newYSize;
            startNoOfHouseholds = newHouseholds;
            households = new List<Household>();
            for (int count = 1; count < startNoOfHouseholds + 1; count++)
            {
                AddHousehold();
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            Settlement s = new SmallSettlement(100,100,20);
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < s.GetNumberOfHouseholds(); j++)
                {
                    
                }
            }
            Console.WriteLine("Households: {0} \nGrid Size: ({1}, {2})",s.GetNumberOfHouseholds(),s.GetXSize(),s.GetYSize());
            Console.WriteLine();
            Console.WriteLine("Press enter to show details");
            Console.ReadLine();
            s.DisplayHouseholds();
            Console.ReadLine();
        }
    }
}
