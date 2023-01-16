using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;






namespace FoodCS
{
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
}