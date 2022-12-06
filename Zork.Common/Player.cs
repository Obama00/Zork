﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<Room> LocationChanged;
        public event EventHandler<int> ScoreChanged;
        public event EventHandler<int> MovesChanged;

        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                    
                }
            }
        }

      
        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
                UpdateMoves();
                if (CurrentRoom.Enemies != null)
                {
                    EnimiesAreInTheRoom = true;
                }
                if (CurrentRoom.Enemies == null)
                {
                    EnimiesAreInTheRoom = false;
                }
            }

            return didMove;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        public void Reward()
        {
            
            Score = Score + 1;
            
            ScoreChanged?.Invoke(this, Score);

        }

        public void UpdateMoves()
        {
           

            Moves = Moves + 1;

            MovesChanged?.Invoke(this, Moves);
            if (EnimiesAreInTheRoom == true)
            {
                foreach (Enemy enemy in CurrentRoom.Enemies)
                {
                    TakeDamage(enemy);
                }

            }
        }
        void TakeDamage(Enemy enemy)
        {
            Health = Health - 1;
            AttackingEnemy = enemy;
            PlayerAttacked = true;
           
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
        public int Score = 0;
        public int Moves = 0;
        public int Health = 10;
        public Enemy AttackingEnemy;
        public bool PlayerAttacked;
        public bool EnimiesAreInTheRoom;
        
    }
}
