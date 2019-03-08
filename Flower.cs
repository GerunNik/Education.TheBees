using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeApp
{
    [Serializable]
    public class Flower
    {
        public int Age;
        public int LifeSpan;
        public double Nectar;
        public double CollectedNectar;
        public bool Alive;
        public Point Location;

        private const int MinLifeExpectancy = 15000;
        private const int MaxLifeExpectancy = 30000;
        private const double StartingNectar = 1.5;
        private const double MaxNectar = 5;
        private const double NectarGrowth = 0.01;
        private const double collectableNectar = 0.3;

        public Flower(Point Ort, Random r)
        {
            Age = 0;
            Nectar = StartingNectar;
            Alive = true;
            LifeSpan = r.Next(MinLifeExpectancy, MaxLifeExpectancy + 1);
            CollectedNectar = 0;
            Location = Ort;
        }

        public double CollectNectar()
        {
            if (Nectar >= collectableNectar)
            {
                Nectar -= collectableNectar;
                CollectedNectar += collectableNectar;
                return collectableNectar;
            }
            return 0;
        }

        public void Walk()
        {
            Age++;
            if (Age <= LifeSpan)
            {
                Nectar += NectarGrowth;
                if (Nectar >= MaxNectar)
                {
                    Nectar = MaxNectar;
                }
            }
            else
            {
                Alive = false;
            }
        }
    }
}
