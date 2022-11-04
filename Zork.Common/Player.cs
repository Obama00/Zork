using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Player
    {


        public Room CurrentRoom
        {
            get => _currentRoom;
            set => _currentRoom = value;
        }

        static public List<Item> Inventory { get; set; }

        [JsonIgnore]
        public Dictionary<string, Item> InventoryByName { get; }

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            Inventory = new List<Item>();
            InventoryByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            foreach (Item item in Inventory)
            {
                InventoryByName.Add(item.Name, item);
            }

        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }
        public void Take(string itemName)
        {

            Item itemToTake = null;

            foreach (Item item in World.Items)
            {
                if (string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemToTake = item;
                    break;
                }
            }
            if (itemToTake == null)
            {
                Game.Output.WriteLine("No such item exists");
                return;
            }
            bool itemIsInRoomInventory = false;
            foreach (Item item in CurrentRoom.Inventory)
            {
                if (item == itemToTake)
                {
                    itemIsInRoomInventory = true;
                    break;
                }
            }

            if (itemIsInRoomInventory == false)
            {
                Game.Output.WriteLine("I can't see any such thing.");
            }
            else
            {
                AddItemToInventory(itemToTake);
                CurrentRoom.RemoveItemFromInventory(itemToTake);
            }
        }

        public void Drop(string itemName)
        {

            Item itemToDrop = null;

            foreach (Item item in World.Items)
            {
                if (string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemToDrop = item;
                    break;
                }
            }
            if (itemToDrop == null)
            {
                Game.Output.WriteLine("No such item exists");
                return;
            }
            bool itemIsInRoomInventory = false;
            foreach (Item item in Inventory)
            {
                if (item == itemToDrop)
                {
                    itemIsInRoomInventory = true;
                    break;
                }
            }

            if (itemIsInRoomInventory == false)
            {
                Game.Output.WriteLine("I can't see any such thing.");
            }
            else
            {
                CurrentRoom.AddItemToInventory(itemToDrop);
                RemoveItemFromInventory(itemToDrop);
            }
        }
        void AddItemToInventory(Item itemToAdd)
        {
            Item takenItem = null;
            foreach (Item item in CurrentRoom.Inventory)
            {
                if (string.Compare(item.Name, itemToAdd.Name, ignoreCase: true) == 0)
                {
                    takenItem = item;
                    Inventory.Add(itemToAdd);
                    break;
                }

            }
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            Item takenItem = null;
            foreach (Item item in Inventory)
            {
                if (string.Compare(item.Name, itemToRemove.Name, ignoreCase: true) == 0)
                {
                    takenItem = item;
                    Inventory.Remove(itemToRemove);
                    break;
                }

            }

        }

        private World _world;
        private Room _currentRoom;
    }
}
