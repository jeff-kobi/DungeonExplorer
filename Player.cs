using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Player
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        private List<string> inventory = new List<string>();

        public Player(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public void PickUpItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                inventory.Add(item);
            }
        }

        public string InventoryContents()
        {
            if (inventory.Count == 0)
            {
                return "Empty";
            }
            return string.Join(", ", inventory);
        }

        public bool HasItem(string item)
        {
            return inventory.Contains(item);
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                Health = Math.Max(0, Health - damage);
            }
        }

        public void Heal(int amount)
        {
            if (amount > 0)
            {
                Health += amount;
            }
        }
    }
}
