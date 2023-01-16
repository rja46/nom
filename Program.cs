
//Skeleton Program code for the AQA A Level Paper 1 Summer 2020 examination
//this code should be used in conjunction with the Preliminary Material
//written by the AQA Programmer Team
//developed in the Visual Studio Community Edition programming environment

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;






namespace FoodCS
{

    class Household
    {
        private static Random rnd = new Random();
        protected double chanceEatOutPerDay;
        protected int xCoord, yCoord, ID;
        protected static int nextID = 1;

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
            details = ID.ToString() + "     Coordinates: (" + xCoord.ToString() + ", " + yCoord.ToString() + ")     Eat out probability: " + chanceEatOutPerDay.ToString();
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
    }

    class Settlement
    {
        private static Random rnd = new Random();
        protected int startNoOfHouseholds, xSize, ySize;
        protected List<Household> households = new List<Household>();

        public Settlement()
        {
            //default values
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
            int counter = 0;
            foreach (var h in households)
            {
                //creates pages of 20
                counter++;
                if (counter % 20 == 0)
                {
                    System.Console.WriteLine("Press Enter...");
                    Console.ReadLine();
                }
                Console.WriteLine(h.GetDetails());
            }
            Console.WriteLine();
        }

        public bool FindOutIfHouseholdEatsOut(int householdNo, ref int x, ref int y)
        {
            double eatOutRNo = rnd.NextDouble();
            x = households[householdNo].GetX();
            y = households[householdNo].GetY();
            //eat out if random number is greater than houses value
            if (eatOutRNo < households[householdNo].GetChanceEatOut())
            {
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
            CreateHouseholds();
        }
    }

    class Outlet
    {
        private static Random rnd = new Random();
        protected int visitsToday, xCoord, yCoord, capacity, maxCapacity;
        protected double dailyCosts;

        public Outlet(int xCoord, int yCoord, int maxCapacityBase)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            capacity = Convert.ToInt32(maxCapacityBase * 0.6);
            maxCapacity = maxCapacityBase + Convert.ToInt32(rnd.NextDouble() * 50) - Convert.ToInt32(rnd.NextDouble() * 50);
            dailyCosts = maxCapacityBase * 0.2 + capacity * 0.5 + 100;
            NewDay();
        }

        public int GetCapacity()
        {
            return capacity;
        }

        public int GetX()
        {
            return xCoord;
        }

        public int GetY()
        {
            return yCoord;
        }

        public void AlterDailyCost(double amount)
        {
            dailyCosts += amount;
        }

        public int AlterCapacity(int change)
        {
            int oldCapacity = capacity;
            capacity += change;
            if (capacity > maxCapacity)
            {
                capacity = maxCapacity;
                return maxCapacity - oldCapacity;
            }
            else if (capacity < 0)
            {
                capacity = 0;
            }
            dailyCosts = maxCapacity * 0.2 + capacity * 0.5 + 100;
            return change;
        }

        public void IncrementVisits()
        {
            visitsToday++;
        }

        public void NewDay()
        {
            visitsToday = 0;
        }

        public double CalculateDailyProfitLoss(double avgCostPerMeal, double avgPricePerMeal)
        {
            return (avgPricePerMeal - avgCostPerMeal) * visitsToday - dailyCosts;
        }

        public string GetDetails()
        {
            string details = "";
            details = "Coordinates: (" + xCoord.ToString() + ", " + yCoord.ToString() + ")     Capacity: " + capacity.ToString() + "      Maximum Capacity: ";
            details += maxCapacity.ToString() + "      Daily Costs: " + dailyCosts.ToString() + "      Visits today: " + visitsToday.ToString();
            return details;
        }
    }

    class Company
    {
        private static Random rnd = new Random();
        protected string name, category;
        protected double balance, reputationScore, avgCostPerMeal, avgPricePerMeal, dailyCosts, familyOutletCost, fastFoodOutletCost, namedChefOutletCost, fuelCostPerUnit, baseCostOfDelivery;
        protected List<Outlet> outlets = new List<Outlet>();
        protected int familyFoodOutletCapacity, fastFoodOutletCapacity, namedChefOutletCapacity;

        public Company(string name, string category, double balance, int x, int y, double fuelCostPerUnit, double baseCostOfDelivery)
        {
            familyOutletCost = 1000;
            fastFoodOutletCost = 2000;
            namedChefOutletCost = 15000;
            familyFoodOutletCapacity = 150;
            fastFoodOutletCapacity = 200;
            namedChefOutletCapacity = 50;
            this.name = name;
            this.category = category;
            this.balance = balance;
            this.fuelCostPerUnit = fuelCostPerUnit;
            this.baseCostOfDelivery = baseCostOfDelivery;
            reputationScore = 100;
            dailyCosts = 100;
            if (category == "fast food")
            {
                avgCostPerMeal = 5;
                avgPricePerMeal = 10;
                reputationScore += rnd.NextDouble() * 10 - 8;
            }
            else if (category == "family")
            {
                avgCostPerMeal = 12;
                avgPricePerMeal = 14;
                reputationScore += rnd.NextDouble() * 30 - 5;
            }
            else
            {
                avgCostPerMeal = 20;
                avgPricePerMeal = 40;
                reputationScore += rnd.NextDouble() * 50;
            }
            OpenOutlet(x, y);
        }

        public string GetName()
        {
            return name;
        }

        public int GetNumberOfOutlets()
        {
            return outlets.Count;
        }

        public double GetReputationScore()
        {
            return reputationScore;
        }

        public void AlterDailyCosts(double change)
        {
            dailyCosts += change;
        }

        public void AlterAvgCostPerMeal(double change)
        {
            avgCostPerMeal += change;
        }

        public void AlterFuelCostPerUnit(double change)
        {
            fuelCostPerUnit += change;
        }

        public void AlterReputation(double change)
        {
            reputationScore += change;
        }

        public void NewDay()
        {
            foreach (var o in outlets)
            {
                o.NewDay();
            }
        }

        public void AddVisitToNearestOutlet(int x, int y)
        {
            int nearestOutlet = 0;
            double nearestOutletDistance, currentDistance;
            nearestOutletDistance = Math.Sqrt((Math.Pow(outlets[0].GetX() - x, 2)) + (Math.Pow(outlets[0].GetY() - y, 2)));
            for (int current = 1; current < outlets.Count; current++)
            {
                currentDistance = Math.Sqrt((Math.Pow(outlets[current].GetX() - x, 2)) + (Math.Pow(outlets[current].GetY() - y, 2)));
                if (currentDistance < nearestOutletDistance)
                {
                    nearestOutletDistance = currentDistance;
                    nearestOutlet = current;
                }
            }
            outlets[nearestOutlet].IncrementVisits();
        }

        public string GetDetails()
        {
            string details = "";
            details += "Name: " + name + "\nType of business: " + category + "\n";
            details += "Current balance: " + balance.ToString() + "\nAverage cost per meal: " + avgCostPerMeal.ToString() + "\n";
            details += "Average price per meal: " + avgPricePerMeal.ToString() + "\nDaily costs: " + dailyCosts.ToString() + "\n";
            details += "Delivery costs: " + CalculateDeliveryCost().ToString() + "\nReputation: " + reputationScore.ToString() + "\n\n";
            details += "Number of outlets: " + outlets.Count.ToString() + "\nOutlets\n";
            for (int current = 1; current < outlets.Count + 1; current++)
            {
                details += current + ". " + outlets[current - 1].GetDetails() + "\n";
            }
            return details;
        }

        public string ProcessDayEnd()
        {
            string details = "";
            double profitLossFromOutlets = 0;
            double profitLossFromThisOutlet = 0;
            double deliveryCosts;
            if (outlets.Count > 1)
            {
                deliveryCosts = baseCostOfDelivery + CalculateDeliveryCost();
            }
            else
            {
                deliveryCosts = baseCostOfDelivery;
            }
            details += "Daily costs for company: " + dailyCosts.ToString() + "\nCost for delivering produce to outlets: " + deliveryCosts.ToString() + "\n";
            for (int current = 0; current < outlets.Count; current++)
            {
                profitLossFromThisOutlet = outlets[current].CalculateDailyProfitLoss(avgCostPerMeal, avgPricePerMeal);
                details += "Outlet " + (current + 1) + " profit/loss: " + profitLossFromThisOutlet.ToString() + "\n";
                profitLossFromOutlets += profitLossFromThisOutlet;
            }
            details += "Previous balance for company: " + balance.ToString() + "\n";
            balance += profitLossFromOutlets - dailyCosts - deliveryCosts;
            details += "New balance for company: " + balance.ToString();
            return details;
        }

        public bool CloseOutlet(int ID)
        {
            bool closeCompany = false;
            outlets.RemoveAt(ID);
            if (outlets.Count == 0)
            {
                closeCompany = true;
            }
            return closeCompany;
        }

        public void ExpandOutlet(int ID)
        {
            int change, result;
            Console.Write("Enter amount you would like to expand the capacity by: ");
            change = Convert.ToInt32(Console.ReadLine());
            result = outlets[ID].AlterCapacity(change);
            if (result == change)
            {
                Console.WriteLine("Capacity adjusted.");
            }
            else
            {
                Console.WriteLine("Only some of that capacity added, outlet now at maximum capacity.");
            }
        }

        public void OpenOutlet(int x, int y)
        {
            int capacity;
            if (category == "fast food")
            {
                balance -= fastFoodOutletCost;
                capacity = fastFoodOutletCapacity;
            }
            else if (category == "family")
            {
                balance -= familyOutletCost;
                capacity = familyFoodOutletCapacity;
            }
            else
            {
                balance -= namedChefOutletCost;
                capacity = namedChefOutletCapacity;
            }
            Outlet newOutlet = new Outlet(x, y, capacity);
            outlets.Add(newOutlet);
        }

        public List<int> GetListOfOutlets()
        {
            List<int> temp = new List<int>();
            for (int current = 0; current < outlets.Count; current++)
            {
                temp.Add(current);
            }
            return temp;
        }

        private double GetDistanceBetweenTwoOutlets(int outlet1, int outlet2)
        {
            return Math.Sqrt((Math.Pow(outlets[outlet1].GetX() - outlets[outlet2].GetX(), 2)) + (Math.Pow(outlets[outlet1].GetY() - outlets[outlet2].GetY(), 2)));
        }

        public double CalculateDeliveryCost()
        {
            List<int> listOfOutlets = new List<int>(GetListOfOutlets());
            double totalDistance = 0;
            double totalCost = 0;
            for (int current = 0; current < listOfOutlets.Count - 1; current++)
            {
                totalDistance += GetDistanceBetweenTwoOutlets(listOfOutlets[current], listOfOutlets[current + 1]);
            }
            totalCost = totalDistance * fuelCostPerUnit;
            return totalCost;
        }
    }

    class Simulation
    {
        private static Random rnd = new Random();
        protected Settlement simulationSettlement;
        protected int noOfCompanies;
        protected double fuelCostPerUnit, baseCostForDelivery;
        protected List<Company> companies = new List<Company>();

        public Simulation()
        {
            fuelCostPerUnit = 0.0098;
            baseCostForDelivery = 100;
            string choice;
            Console.Write("Enter L for a large settlement, anything else for a normal size settlement: ");
            choice = Console.ReadLine();
            if (choice == "L")
            {
                int extraX, extraY, extraHouseholds;
                Console.Write("Enter additional amount to add to X size of settlement: ");
                extraX = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter additional amount to add to Y size of settlement: ");
                extraY = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter additional number of households to add to settlement: ");
                extraHouseholds = Convert.ToInt32(Console.ReadLine());
                simulationSettlement = new LargeSettlement(extraX, extraY, extraHouseholds);
            }
            else
            {
                simulationSettlement = new Settlement();
            }
            Console.Write("Enter D for default companies, anything else to add your own start companies: ");
            choice = Console.ReadLine();
            if (choice == "D")
            {
                noOfCompanies = 3;
                Company company1 = new Company("AQA Burgers", "fast food", 100000, 200, 203, fuelCostPerUnit, baseCostForDelivery);
                companies.Add(company1);
                companies[0].OpenOutlet(300, 987);
                companies[0].OpenOutlet(500, 500);
                companies[0].OpenOutlet(305, 303);
                companies[0].OpenOutlet(874, 456);
                companies[0].OpenOutlet(23, 408);
                companies[0].OpenOutlet(412, 318);
                Company company2 = new Company("Ben Thor Cuisine", "named chef", 100400, 390, 800, fuelCostPerUnit, baseCostForDelivery);
                companies.Add(company2);
                Company company3 = new Company("Paltry Poultry", "fast food", 25000, 800, 390, fuelCostPerUnit, baseCostForDelivery);
                companies.Add(company3);
                companies[2].OpenOutlet(400, 390);
                companies[2].OpenOutlet(820, 370);
                companies[2].OpenOutlet(800, 600);
            }
            else
            {
                Console.Write("Enter number of companies that exist at start of simulation: ");
                noOfCompanies = Convert.ToInt32(Console.ReadLine());
                for (int count = 1; count < noOfCompanies + 1; count++)
                {
                    AddCompany();
                }
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("**********    MENU     **********");
            Console.WriteLine("*********************************");
            Console.WriteLine("1. Display details of households");
            Console.WriteLine("2. Display details of companies");
            Console.WriteLine("3. Modify company");
            Console.WriteLine("4. Add new company");
            Console.WriteLine("6. Advance to next day");
            Console.WriteLine("Q. Quit");
            Console.Write("\n Enter your choice: ");
        }

        private void DisplayCompaniesAtDayEnd()
        {
            string details;
            Console.WriteLine("\n**********************");
            Console.WriteLine("***** Companies: *****");
            Console.WriteLine("**********************\n");
            foreach (var c in companies)
            {
                Console.WriteLine(c.GetName());
                Console.WriteLine();
                details = c.ProcessDayEnd();
                Console.WriteLine(details + "\n");
            }
        }

        private void ProcessAddHouseholdsEvent()
        {
            int NoOfNewHouseholds = rnd.Next(1, 5);
            for (int i = 1; i < NoOfNewHouseholds + 1; i++)
            {
                simulationSettlement.AddHousehold();
            }
            Console.WriteLine(NoOfNewHouseholds.ToString() + " new households have been added to the settlement.");
        }

        private void ProcessCostOfFuelChangeEvent()
        {
            double fuelCostChange = rnd.Next(1, 10) / 10.0;
            int upOrDown = rnd.Next(0, 2);
            int companyNo = rnd.Next(0, companies.Count);
            if (upOrDown == 0)
            {
                Console.WriteLine("The cost of fuel has gone up by " + fuelCostChange.ToString() + " for " + companies[companyNo].GetName());
            }
            else
            {
                Console.WriteLine("The cost of fuel has gone down by " + fuelCostChange.ToString() + " for " + companies[companyNo].GetName());
                fuelCostChange *= -1;
            }
            companies[companyNo].AlterFuelCostPerUnit(fuelCostChange);
        }

        private void ProcessReputationChangeEvent()
        {
            double reputationChange = rnd.Next(1, 10) / 10.0;
            int upOrDown = rnd.Next(0, 2);
            int companyNo = rnd.Next(0, companies.Count);
            if (upOrDown == 0)
            {
                Console.WriteLine("The reputation of " + companies[companyNo].GetName() + " has gone up by " + reputationChange.ToString());
            }
            else
            {
                Console.WriteLine("The reputation of " + companies[companyNo].GetName() + " has gone down by " + reputationChange.ToString());
                reputationChange *= -1;
            }
            companies[companyNo].AlterReputation(reputationChange);
        }

        private void ProcessCostChangeEvent()
        {
            double costToChange = rnd.Next(0, 2);
            int upOrDown = rnd.Next(0, 2);
            int companyNo = rnd.Next(0, companies.Count);
            double amountOfChange;
            if (costToChange == 0)
            {
                amountOfChange = rnd.Next(1, 20) / 10.0;
                if (upOrDown == 0)
                {
                    Console.WriteLine("The daily costs for " + companies[companyNo].GetName() + " have gone up by " + amountOfChange.ToString());
                }
                else
                {
                    Console.WriteLine("The daily costs for " + companies[companyNo].GetName() + " have gone down by " + amountOfChange.ToString());
                    amountOfChange *= -1;
                }
                companies[companyNo].AlterDailyCosts(amountOfChange);
            }
            else
            {
                amountOfChange = rnd.Next(1, 10) / 10.0;
                if (upOrDown == 0)
                {
                    Console.WriteLine("The average cost of a meal for " + companies[companyNo].GetName() + " has gone up by " + amountOfChange.ToString());
                }
                else
                {
                    Console.WriteLine("The average cost of a meal for " + companies[companyNo].GetName() + " has gone down by " + amountOfChange.ToString());
                    amountOfChange *= -1;
                }
                companies[companyNo].AlterAvgCostPerMeal(amountOfChange);
            }
        }

        private void DisplayEventsAtDayEnd()
        {
            Console.WriteLine("\n***********************");
            Console.WriteLine("*****   Events:   *****");
            Console.WriteLine("***********************\n");
            double eventRanNo;
            eventRanNo = rnd.NextDouble();
            if (eventRanNo < 0.25)
            {
                eventRanNo = rnd.NextDouble();
                if (eventRanNo < 0.25)
                {
                    ProcessAddHouseholdsEvent();
                }
                eventRanNo = rnd.NextDouble();
                if (eventRanNo < 0.5)
                {
                    ProcessCostOfFuelChangeEvent();
                }
                eventRanNo = rnd.NextDouble();
                if (eventRanNo < 0.5)
                {
                    ProcessReputationChangeEvent();
                }
                eventRanNo = rnd.NextDouble();
                if (eventRanNo >= 0.5)
                {
                    ProcessCostChangeEvent();
                }
            }
            else
            {
                Console.WriteLine("No events.");
            }
        }

        public void ProcessDayEnd()
        {
            double totalReputation = 0;
            List<double> reputations = new List<double>();
            int companyRNo, current, loopMax, x = 0, y = 0;
            foreach (var c in companies)
            {
                c.NewDay();
                totalReputation += c.GetReputationScore();
                reputations.Add(totalReputation);
            }
            loopMax = simulationSettlement.GetNumberOfHouseholds() - 1;
            for (int counter = 0; counter < loopMax + 1; counter++)
            {
                if (simulationSettlement.FindOutIfHouseholdEatsOut(counter, ref x, ref y))
                {
                    companyRNo = rnd.Next(1, Convert.ToInt32(totalReputation) + 1);
                    current = 0;
                    while (current < reputations.Count)
                    {
                        if (companyRNo < reputations[current])
                        {
                            companies[current].AddVisitToNearestOutlet(x, y);
                            break;
                        }
                        current++;
                    }
                }
            }
            DisplayCompaniesAtDayEnd();
            DisplayEventsAtDayEnd();
        }

        private void AddCompany()
        {
            int balance, x = 0, y = 0;
            string companyName, typeOfCompany = "9";
            Console.Write("Enter a name for the company: ");
            companyName = Console.ReadLine();
            Console.Write("Enter the starting balance for the company: ");
            balance = Convert.ToInt32(Console.ReadLine());
            while (typeOfCompany != "1" && typeOfCompany != "2" && typeOfCompany != "3")
            {
                Console.Write("Enter 1 for a fast food company, 2 for a family company or 3 for a named chef company: ");
                typeOfCompany = Console.ReadLine();
            }
            if (typeOfCompany == "1")
            {
                typeOfCompany = "fast food";
            }
            else if (typeOfCompany == "2")
            {
                typeOfCompany = "family";
            }
            else
            {
                typeOfCompany = "named chief";
            }
            simulationSettlement.GetRandomLocation(ref x, ref y);
            Company newCompany = new Company(companyName, typeOfCompany, balance, x, y, fuelCostPerUnit, baseCostForDelivery);
            companies.Add(newCompany);
        }

        public int GetIndexOfCompany(string companyName)
        {
            int index = -1;
            for (int current = 0; current < companies.Count; current++)
            {
                if (companies[current].GetName().ToLower() == companyName.ToLower())
                {
                    return current;
                }
            }
            return index;
        }

        public void ModifyCompany(int index)
        {
            string choice;
            int outletIndex, x, y;
            bool closeCompany;
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*******  MODIFY COMPANY   *******");
            Console.WriteLine("*********************************");
            Console.WriteLine("1. Open new outlet");
            Console.WriteLine("2. Close outlet");
            Console.WriteLine("3. Expand outlet");
            Console.Write("\nEnter your choice: ");
            choice = Console.ReadLine();
            if (choice == "2" || choice == "3")
            {
                Console.Write("Enter ID of outlet: ");
                outletIndex = Convert.ToInt32(Console.ReadLine());
                if (outletIndex > 0 && outletIndex <= companies[index].GetNumberOfOutlets())
                {
                    if (choice == "2")
                    {
                        closeCompany = companies[index].CloseOutlet(outletIndex - 1);
                        if (closeCompany)
                        {
                            Console.WriteLine("That company has now closed down as it has no outlets.");
                            companies.RemoveAt(index);
                        }
                    }
                    else
                    {
                        companies[index].ExpandOutlet(outletIndex - 1);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid outlet ID.");
                }
            }
            else if (choice == "1")
            {
                Console.Write("Enter X coordinate for new outlet: ");
                x = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Y coordinate for new outlet: ");
                y = Convert.ToInt32(Console.ReadLine());
                if (x >= 0 && x < simulationSettlement.GetXSize() && y >= 0 && y < simulationSettlement.GetYSize())
                {
                    companies[index].OpenOutlet(x, y);
                }
                else
                {
                    Console.WriteLine("Invalid coordinates.");
                }
            }
            Console.WriteLine();
        }

        public void DisplayCompanies()
        {
            Console.WriteLine("\n*********************************");
            Console.WriteLine("*** Details of all companies: ***");
            Console.WriteLine("*********************************\n");
            foreach (var c in companies)
            {
                Console.WriteLine(c.GetDetails() + "\n");
            }
            Console.WriteLine();
        }

        public void Run()
        {
            string choice = "";
            int index;
            while (choice != "Q")
            {
                DisplayMenu();
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        simulationSettlement.DisplayHouseholds();
                        break;
                    case "2":
                        DisplayCompanies();
                        break;
                    case "3":
                        string companyName;
                        index = -1;
                        while (index == -1)
                        {
                            Console.Write("Enter company name: ");
                            companyName = Console.ReadLine();
                            index = GetIndexOfCompany(companyName);
                        }
                        ModifyCompany(index);
                        break;
                    case "4":
                        AddCompany();
                        break;
                    case "6":
                        ProcessDayEnd();
                        break;
                    case "Q":
                        Console.WriteLine("Simulation finished, press Enter to close.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Settlement s = new SmallSettlement(100,100,10);

            Console.WriteLine("Households: {0} \nGrid Size: ({1}, {2})",s.GetNumberOfHouseholds(),s.GetXSize(),s.GetYSize()); 

            Console.WriteLine(); 

            Console.WriteLine("Press enter to show details"); 

            Console.ReadLine(); 

            s.DisplayHouseholds(); 

            Console.ReadLine(); 
        }
    }
}