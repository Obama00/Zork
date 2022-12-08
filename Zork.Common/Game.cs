using System;
using System.Linq;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }

        [JsonIgnore]
        public Enemy Enemy { get; set; }
        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        
        
        }

        public void Run(IInputService input, IOutputService output )
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
            

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            

            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
            



        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
            }
            else if (commandTokens.Length == 1)
            {
                verb = commandTokens[0];
            }
            else
            {
                verb = commandTokens[0];
                subject = commandTokens[1];
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    Player.UpdateMoves();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    break;
                case Commands.Reward:
                    Player.RewardGain();
                    Player.UpdateMoves();
                    Output.WriteLine("You gained a point");
                    break;


                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                        Player.UpdateMoves();
                    }
                    break;
                case Commands.Rock:
                    if (Player.CurrentRoom == Player.Hades)
                    {
                        RPS(Commands.Rock);
                    }
                    if (Player.CurrentRoom != Player.Hades)
                    {
                        Output.WriteLine("This is no time to play silly games");
                    }

                    break;

                case Commands.Paper:
                    if (Player.CurrentRoom == Player.Hades)
                    {
                        RPS(Commands.Paper);
                    }
                    if (Player.CurrentRoom != Player.Hades)
                    {
                        Output.WriteLine("This is no time to play silly games");
                    }

                    break;

                case Commands.Scissors:
                    if (Player.CurrentRoom == Player.Hades)
                    {
                        RPS(Commands.Scissors);
                    }
                    if (Player.CurrentRoom != Player.Hades)
                    {
                        Output.WriteLine("This is no time to play silly games");
                    }

                    break;

                case Commands.Attack:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a target");
                    }
                    else
                    {
                        Attacking(subject);
                        
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                        Player.UpdateMoves();
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                        Player.UpdateMoves();
                    }
                    else
                    {
                        Player.UpdateMoves();
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    break;
                case Commands.Score:
                    Player.UpdateMoves();
                    string scoretext = Player.Score.ToString();
                    string movetext = Player.Moves.ToString();
                    Output.WriteLine($"Your score is {scoretext}, in {movetext} move(s)");
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (Player.PlayerAttacked == true)
            {
                Attacked();
            }

            if (Player.Health == 0)
            {
                if (Player.CurrentRoom != Player.Hades)
                {
                    Output.WriteLine("You died!");
                }
                Player.CurrentRoom = Player.Hades;
                Player.EnimiesAreInTheRoom = false;
            }
           
            if (Player.Score >= 3)
            {
                if (Player.CurrentRoom == Player.Hades)
                {
                    Respawn();
                }
            }

            Output.WriteLine($"\n{Player.CurrentRoom}");
            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }



        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }

            foreach (Enemy enemy in Player.CurrentRoom.Enemies)
            {
                Output.WriteLine(enemy.LookDescription);
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine("Taken.");
            }
        }

        private void Attacking(string enemyName)
        {
            Random random = new Random();
            Enemy enemyToAttack = Player.CurrentRoom.Enemies.FirstOrDefault(enemy => string.Compare(enemy.Name, enemyName, ignoreCase: true) == 0);
            int x = random.Next(0, 100);
            if (enemyToAttack == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            { if (x <= 80)
                {
                    enemyToAttack.TakeDamage(1);
                    Output.WriteLine($"You attacked the {enemyToAttack.Name}!");
                    if (enemyToAttack.Health <= 0)
                    {
                        Output.WriteLine($"You defeated the {enemyToAttack.Name}!");
                        Player.CurrentRoom.RemoveEnemyFromRoom(enemyToAttack);

                    }
                }
              if (x > 80 & x <= 85)
                {
                    Output.WriteLine($"You landed a critical hit on {enemyToAttack.Name}");
                    enemyToAttack.TakeDamage(2);
                }
              if (x > 85)
                {
                    Output.WriteLine("You missed the attack!");
                }
            }
            Player.UpdateMoves();
        }

       
        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                   Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine("Dropped.");
            }
        }

        private void Attacked()
        {
            Output.WriteLine($"A {Player.AttackingEnemy.Name} attacked you!");
            Player.PlayerAttacked = false;
        }

        private void Respawn()
        {
            Output.WriteLine("You have come back to life");
            Player.CurrentRoom = Player.StartingLocation;
            Player.Health = 1;
            Player.Score = 0;

        }
        private void RPS(Commands command)
        {
            Random random = new Random();
            int x = random.Next(0, 100);
            if(x <= 34)
            {
                Output.WriteLine("The shadowy figure played Scissors");
                if (command == Commands.Rock)
                {
                    Output.WriteLine("You've bested the Shadowy figure");
                    Player.RewardGain();
                    Output.WriteLine("You gained a point");

                }

                if (command == Commands.Paper)
                {
                    Output.WriteLine("You lost to the shadowy figure");
                    Player.RewardLose();
                    Output.WriteLine("You lost a point");

                }

                if (command == Commands.Scissors)
                {
                    Output.WriteLine("It was a draw.");
                    Output.WriteLine("Try again.");

                }

            }

            if (x > 34 & x <= 67)
            {
                Output.WriteLine("The shadowy figure played Rock");
                if (command == Commands.Paper)
                {
                    Output.WriteLine("You've bested the Shadowy figure");
                    Player.RewardGain();
                    Output.WriteLine("You gained a point");

                }

                if (command == Commands.Scissors)
                {
                    Output.WriteLine("You lost to the shadowy figure");
                    Player.RewardLose();
                    Output.WriteLine("You lost a point");

                }
                if (command == Commands.Rock)
                {
                    Output.WriteLine("It was a draw.");
                    Output.WriteLine("Try again.");

                }

            }

            if (x > 67)
            {
                Output.WriteLine("The shadowy figure played Paper");
                if (command == Commands.Scissors)
                {
                    Output.WriteLine("You've bested the Shadowy figure");
                    Player.RewardGain();
                    Output.WriteLine("You gained a point");

                }

                if (command == Commands.Rock)
                {
                    Output.WriteLine("You lost to the shadowy figure");
                    Player.RewardLose();
                    Output.WriteLine("You lost a point");

                }
                if (command == Commands.Paper)
                {
                    Output.WriteLine("It was a draw.");
                    Output.WriteLine("Try again.");

                }

            }
            Player.UpdateMoves();
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}