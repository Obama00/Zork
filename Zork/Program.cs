using System;

namespace Zork
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;
            bool isRunning = true;
            while (isRunning)
                {

                while (command != Commands.QUIT)
                {
                    Console.Write($"{_rooms[_currentRoom]}\n> ");
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

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        
        private static bool Move(Commands command)
        {
            bool didMove = false;
            switch (command)
            {
                case Commands.NORTH:
                case Commands.SOUTH:
                    didMove = false;
                    break;
                case Commands.EAST when _currentRoom < _rooms.Length -1:
                    _currentRoom++;
                    didMove = true;
                    break;
                case Commands.WEST:
                    if (_currentRoom > 0)
                    {
                        _currentRoom--;
                        didMove = true;
                    }
                    break;
            }
            return didMove;
        }

        private static string[] _rooms = { "Forest", "West of house", "Behind House", "Clearing", "Canyon View" };
        private static int _currentRoom = 1;

    }
}