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
                    Console.Write("> ");
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
                            outputString = $"You moved {command}.";
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
       

    }
}