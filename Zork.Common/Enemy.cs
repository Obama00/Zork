

namespace Zork.Common
{
     public  class Enemy
    {
        public string Name { get; }
        

        public string LookDescription { get; }

        public int Health { get; private set; }
        
        public string CurrentRoom { get; private set; }

        public Player Player;

        

        public Enemy(string name, string lookDescription, int health, string currentRoom)
        {
            CurrentRoom = currentRoom;
            Name = name;
            LookDescription = lookDescription;
            Health = health;
        
        }

        

        public override string ToString() => Name;

        
      public void TakeDamage(int Damage)
        {
            Health = Health - Damage;
            


        }

       
    }
}

