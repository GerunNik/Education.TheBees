using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeApp
{
    [Serializable]
    public class Bee
    {
        const int Stepwidth = 3;
        const int WorkAge = 1000;
        const double HoneyConsumption = 0.5;
        const double MinNectarInFlower = 1.5;
        private Nest Nest;
        private World World;

        public int Age { get; private set; }
        public bool InNest { get; private set; }
        public double CollectedNectar { get; private set; }

        private Point place;
        public Point Place { get { return place; } }

        public int ID;
        private Flower TargetFlower;

        public BeeStatus State = BeeStatus.Useless;

        public Bee(int id, Point place, World world, Nest nest)
        {
            this.ID = id;
            this.place = place;
            Age = 0;
            CollectedNectar = 0;
            InNest = true;
            TargetFlower = null;
            World = world;
            Nest = nest;
        }

        public void Walk()
        {
            Age++;
            switch (State)
            {
                case BeeStatus.Useless:
                    if (Age > WorkAge)
                    {
                        State = BeeStatus.InRetirement;
                    }
                    else if (World.Flowers.Count > 0 && Nest.UseHoney(HoneyConsumption))
                    {
                        Flower flower = World.Flowers[new Random().Next(World.Flowers.Count)];
                        if (flower.Nectar <= MinNectarInFlower && flower.Alive)
                        {
                            TargetFlower = flower;
                            State = BeeStatus.FlyingToFlower;
                        }
                    }
                    break;

                case BeeStatus.FlyingToFlower:
                    if (!World.Flowers.Contains(TargetFlower))
                    {
                        State = BeeStatus.FlyingToNest;
                    }
                    else if (InNest)
                    {
                        if (MoveToLocation(Nest.LookupPlace("Exit")))
                        {
                            InNest = false;
                            place = Nest.LookupPlace("Entrance");
                        }
                    }
                    else
                    {
                        if (MoveToLocation(TargetFlower.Location))
                        {
                            State = BeeStatus.CollectingNectar;
                        }
                    }
                    break;

                case BeeStatus.CollectingNectar:
                    double nectar = TargetFlower.CollectNectar();
                    if (nectar > 0)
                    {
                        CollectedNectar += nectar;
                    }
                    else
                    {
                        State = BeeStatus.FlyingToNest;
                    }
                    break;

                case BeeStatus.FlyingToNest:
                    if (!InNest)
                    {
                        if (MoveToLocation(Nest.LookupPlace("HoneyFactory")))
                        {
                            InNest = true;
                            place = Nest.LookupPlace("Exit");
                        }
                    }
                    else
                    {
                        if (Nest.AddHoney(0.5))
                        {
                            CollectedNectar -= 0.5;
                        }
                        else
                        {
                            CollectedNectar = 0;
                        }
                    }
                    break;

                case BeeStatus.ProducingHoney:
                    if (CollectedNectar < 0.5)
                    {
                        CollectedNectar = 0;
                        State = BeeStatus.Useless;
                    }
                    break;

                case BeeStatus.InRetirement:
                    //Dont fecking Move
                    break;
            }
        }

        bool MoveToLocation(Point Target)
        {
            if (Target != null)
            {
                if (Math.Abs(Target.X - place.X) <= Stepwidth &&
                    Math.Abs(Target.Y - place.Y) <= Stepwidth)
                {
                    return true;
                }
                if (Target.X > place.X)
                {
                    place.X += Stepwidth;
                }
                else if (Target.X < place.X)
                {
                    place.X -= Stepwidth;
                }

                if (Target.Y > place.Y)
                {
                    place.Y += Stepwidth;
                }
                else if (Target.Y < place.Y)
                {
                    place.Y -= Stepwidth;
                }
            }
            return false;
        }

        void currentStatus()
        {
            
        }
    }
}
