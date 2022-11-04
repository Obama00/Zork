﻿using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public static IOutputService Output { get; set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IOutputService output)
        {

            Output = output;

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);
                    foreach (Item item in Player.CurrentRoom.Inventory)
                    {

                        Output.WriteLine(item.Description);

                    }
                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                // might look like:  "LOOK", "TAKE MAT", "QUIT"
                char separator = ' ';
                string[] commandTokens = inputString.Split(separator);

                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
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

                Commands command = ToCommand(verb);
                string outputString;
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.Look:
                        outputString = Player.CurrentRoom.Description;
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {

                            outputString += item.Description;

                        }

                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.Take:
                        Player.Take(subject);
                        //TODO

                        outputString = "Taken";
                        break;

                    case Commands.Drop:
                        Player.Drop(subject);
                        //TODO
                        outputString = "Dropped";
                        break;

                    case Commands.Inventory:
                        outputString = null;
                        foreach (Item item in Player.Inventory)
                        {

                            outputString = item.Description;

                        }

                        break;


                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Output.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}
