using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeApp
{
    [Serializable]
    public class World
    {
        private const double something = 50;

        private const int FieldMinX = 15;
        private const int FieldMinY = 177;
        private const int FieldMaxX = 690;
        private const int FieldMaxY = 290;

        public Nest Nest;
        public List<Bee> Bees;
        public List<Flower> Flowers;

        public World()
        {
            Bees = new List<Bee>();
            Flowers = new List<Flower>();
            Nest = new Nest(this);
            Random r = new Random();

            for (int i = 0; i < 10; i++)
            {
                NewFlower(r);
            }
        }

        public void Walk()
        {
            Nest.Walk();

            for (int i = Bees.Count - 1; i >= 0; i--)
            {
                Bees[i].Walk();
                if (Bees[i].State == BeeStatus.InRetirement)
                {
                    Bees.Remove(Bees[i]);
                }
            }

            double allCollectedNectar = 0;
            for (int i = Flowers.Count - 1; i >= 0; i--)
            {
                Flowers[i].Walk();
                allCollectedNectar += Flowers[i].CollectedNectar;
                if (!Flowers[i].Alive)
                {
                    Flowers.Remove(Flowers[i]);
                }
            }
        }

        private void NewFlower(Random r)
        {
            Point place = new Point(r.Next(FieldMinX, FieldMaxX), new Random().Next(FieldMinY, FieldMaxY));

            Flowers.Add(new Flower(place, r));
        }
    }
}
