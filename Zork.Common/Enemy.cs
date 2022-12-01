using System;
using System.Collections.Generic;
using System.Text;

namespace Zork.Common
{
     public  class Enemy
    {
        public string Name { get; }

        public string LookDescription { get; }

        public int Health { get; private set; }

        public Enemy(string name, string lookDescription, int health)
        {
            Name = name;
            LookDescription = lookDescription;
            Health = health;
        }

        public override string ToString() => Name;
    }
}

