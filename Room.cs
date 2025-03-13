using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Room
    {
        public string Id { get; private set; }
        private string description;
        private Dictionary<string, string> exits;
        private List<string> items;
        private List<Creature> creatures;

        public Room(string description, string id)
        {
            this.description = description;
            this.Id = id;
            exits = new Dictionary<string, string>();
            items = new List<string>();
            creatures = new List<Creature>();
        }

        public string GetDescription()
        {
            return description;
        }

        public void AddExit(string direction, string roomId)
        {
            if (!exits.ContainsKey(direction))
            {
                exits.Add(direction, roomId);
            }
        }

        public Dictionary<string, string> GetExits()
        {
            return exits;
        }

        public void AddItem(string item)
        {
            items.Add(item);
        }

        public bool HasItem(string item)
        {
            return items.Contains(item.ToLower());
        }

        public void RemoveItem(string item)
        {
            items.Remove(item);
        }

        public List<string> GetItems()
        {
            return items;
        }

        public void AddCreature(Creature creature)
        {
            creatures.Add(creature);
        }

        public void RemoveCreature(Creature creature)
        {
            creatures.Remove(creature);
        }

        public List<Creature> GetCreatures()
        {
            return creatures;
        }
    }
}
