using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeApp
{
    [Serializable]
    public class Nest
    {
        private int AmountOfBees;
        public double Honey;
        private Dictionary<string, Point> Places = new Dictionary<string, Point>();
        private World World;

        private const int StartingNumberOfBees = 6;
        private const double startingHoney = 3.2;
        private const double MaxAmountOfHoney = 15;
        private const double ConversionrateNectarToHoney = 0.25;
        private const int maxNumberOfBees = 8;
        private const double MinimumNectarForBee = 4;

        public Nest(World world)
        {
            Honey = startingHoney;
            InitializingPlaces();
            World = world;
            while (AmountOfBees < StartingNumberOfBees)
            {
                NewBee(new Random());
            }
        }

        private void InitializingPlaces()
        {
            Places.Add("Entrance", new Point(600, 100));
            Places.Add("ChildrensPlayground", new Point(95, 174));
            Places.Add("HoneyFactory", new Point(157, 98));
            Places.Add("Exit", new Point(194, 213));
        }

        public bool AddHoney(double nectar)
        {
            Honey += nectar * ConversionrateNectarToHoney;

            if (Honey > MaxAmountOfHoney)
            {
                Honey = MaxAmountOfHoney;
                return false;
            }

            return true;
        }

        public bool UseHoney(double amount)
        {
            if (amount > Honey)
            {
                return false;
            }
            else
            {
                Honey -= amount;
                return true;
            }
        }

        public void NewBee(Random r)
        {
            if (AmountOfBees < maxNumberOfBees)
            {
                int r1 = r.Next(100) - 50;
                int r2 = r.Next(100) - 50;

                Point spawnPoint = new Point(Places["ChildrensPlayground"].X + r1, Places["ChildrensPlayground"].Y + r2);
                
                World.Bees.Add(new Bee(AmountOfBees, spawnPoint, World, this));
                AmountOfBees++;
            }
        }

        public void Walk()
        {
            if (Honey > MinimumNectarForBee && new Random().Next(10) == 1)
            {
                NewBee(new Random());
            }
        }

        public Point LookupPlace(string PlaceToFind)
        {
            foreach (var item in Places)
            {
                if (item.Key == PlaceToFind)
                {
                    return item.Value;
                }
            }

            throw new ArgumentException();
        }
    }
}
