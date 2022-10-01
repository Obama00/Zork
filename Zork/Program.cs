using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    internal class Program
    {
        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }
        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }
        static void Main(string[] args)
        {
            const string defaultRoomsFilename = "Rooms.json";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            Console.WriteLine("Welcome to Zork!");
            InitializeRooms(roomsFilename);
            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            bool isRunning = true;
            while (isRunning)
            {
                while (command != Commands.QUIT)
                {
                    Console.WriteLine(CurrentRoom);
                    if (previousRoom != CurrentRoom)
                    {
                        Console.WriteLine(CurrentRoom.Description);
                        previousRoom = CurrentRoom;
                    }
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
                            outputString = CurrentRoom.Description;
                            break;
                        case Commands.NORTH:
                        case Commands.SOUTH:
                        case Commands.EAST:
                        case Commands.WEST:
                            if (Move(command))
                            {
                                outputString = $"You moved {command}";
                            }
                            else
                            {
                                outputString = "The way is shut!";

                            }
                            break;
                        default:
                            outputString = "Unknown Command";
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
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    didMove = true;
                    break;
                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    didMove = true;
                    break;
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    didMove = true;
                    break;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    didMove = true;
                    break;
            }
            return didMove;
        }
        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static Room[,] Rooms;

        private static readonly List<Commands> Directions = new List<Commands>
    {
        Commands.NORTH,
        Commands.SOUTH,
        Commands.EAST,
        Commands.WEST,
    };
        private static (int Row, int Column) Location = (1, 1);
        private enum Fields
        {
            Name = 0,
            Description
        }
        private static void InitializeRooms(string roomsFilename) =>
            Rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFilename));


    }

}
