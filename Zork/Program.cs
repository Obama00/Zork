using System;
using System.Collections.Generic;

namespace Zork
{

   internal class Program
    {
        private static string CurrentRoom
        {
            get
            {
                return _rooms[Location.Row, Location.Column];
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;
            bool isRunning = true;
            while (isRunning)
                {

                while (command != Commands.QUIT)
                {
                    Console.WriteLine(CurrentRoom);
                    Console.Write(">");
                    command = ToCommand(Console.ReadLine().Trim());

                    string outputString;
                    switch (command)
                    {
                        case Commands.QUIT:
                            isRunning = false;
                            outputString = "Thank you for playing!";
                            break;

                        case Commands.LOOK:
                            outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                            break;

                        case Commands.NORTH:
                        case Commands.SOUTH:
                        case Commands.EAST:
                        case Commands.WEST:
                            if (Move(command))
                            {
                                outputString = $"You moved {command}.";
                            }
                            else
                            {
                                outputString = "The way is shut";
                            }
                            
                            break;
                        default:
                            outputString = "Unknown command.";
                            break;
                    };
                    Console.WriteLine(outputString);
                }
            }
        }

      
        
        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid Direction.");
            bool didMove = false;
            switch (command)
            {
                case Commands.NORTH when Location.Row < _rooms.GetLength(0) - 1:
                    Location.Row++;
                    didMove = true;
                    break;
                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    didMove = true;
                    break;
                case Commands.EAST when Location.Column < _rooms.GetLength(1) -1:
                    Location.Column++;
                    didMove = true;
                    break;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                   
                    break;
            }
            return didMove;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static readonly string[,] _rooms = {
            {"Rocky Trail", "South of House", "Canyon View"},
        {"Forest", "West of House", "Behind House" },
        {"Dense Woods", "North of House", "Clearing" } };
       

        private static readonly List<Commands> Directions = new List<Commands>
    {
        Commands.NORTH,
        Commands.SOUTH,
        Commands.EAST,
        Commands.WEST,
    };
        private static (int Row, int Column) Location = (1, 1);
    }
   
}
