using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Game
    {
        private Player player;
        private Dictionary<string, Room> rooms;
        private Room currentRoom;
        private bool treasureFound = false;

        public Game()
        {
            // Initialize the game with multiple rooms and one player
            InitializeRooms();

            Console.WriteLine("Welcome to Dungeon Explorer!");
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();
            player = new Player(playerName, 100);
            Console.WriteLine($"Welcome, {player.Name}! Your adventure begins...");
            Console.WriteLine("Find the treasure and escape the dungeon to win!");

            // Start in the entrance
            currentRoom = rooms["entrance"];
        }

        private void InitializeRooms()
        {
            rooms = new Dictionary<string, Room>();

            // Create rooms
            rooms.Add("entrance", new Room("You are at the entrance of a dark, damp dungeon. There's a torch on the wall.", "entrance"));
            rooms.Add("corridor", new Room("A long corridor stretches before you. Water drips from the ceiling.", "corridor"));
            rooms.Add("armory", new Room("An old armory with weapon racks. Most weapons are rusted beyond use.", "armory"));
            rooms.Add("hall", new Room("A grand hall with pillars. Seems to have been a gathering place once.", "hall"));
            rooms.Add("treasury", new Room("A small room with a pedestal in the center. A golden chalice sits on it!", "treasury"));
            rooms.Add("exit", new Room("Sunlight streams in from an opening. Freedom awaits!", "exit"));

            // Set up connections (directions)
            rooms["entrance"].AddExit("north", "corridor");

            rooms["corridor"].AddExit("south", "entrance");
            rooms["corridor"].AddExit("east", "armory");
            rooms["corridor"].AddExit("north", "hall");

            rooms["armory"].AddExit("west", "corridor");

            rooms["hall"].AddExit("south", "corridor");
            rooms["hall"].AddExit("east", "treasury");
            rooms["hall"].AddExit("north", "exit");

            rooms["treasury"].AddExit("west", "hall");

            rooms["exit"].AddExit("south", "hall");

            // Add items to rooms
            rooms["entrance"].AddItem("torch");
            rooms["armory"].AddItem("rusty sword");
            rooms["armory"].AddItem("cracked shield");
            rooms["treasury"].AddItem("golden chalice");

            // Add creatures
            rooms["corridor"].AddCreature(new Creature("Giant Rat", 20, 5));
            rooms["hall"].AddCreature(new Creature("Skeleton Warrior", 40, 10));
        }

        public void Start()
        {
            bool playing = true;

            // Display initial room description
            Console.WriteLine("\n" + currentRoom.GetDescription());

            while (playing)
            {
                Console.WriteLine("\n=== Dungeon Explorer ===");
                Console.WriteLine("1. Look around");
                Console.WriteLine("2. Check player status");
                Console.WriteLine("3. Move to another room");
                Console.WriteLine("4. Pick up item");
                Console.WriteLine("5. Battle creature");
                Console.WriteLine("6. Exit game");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LookAround();
                        break;
                    case "2":
                        DisplayPlayerStatus();
                        break;
                    case "3":
                        Move();
                        break;
                    case "4":
                        TryPickUpItem();
                        break;
                    case "5":
                        BattleCreature();
                        break;
                    case "6":
                        playing = false;
                        Console.WriteLine("Thank you for playing Dungeon Explorer!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                // Check victory condition
                if (currentRoom.Id == "exit" && player.HasItem("golden chalice"))
                {
                    Console.WriteLine("\nCONGRATULATIONS! You've escaped the dungeon with the treasure!");
                    Console.WriteLine("You win the game!");
                    playing = false;
                }

                // Check defeat condition
                if (player.Health <= 0)
                {
                    Console.WriteLine("\nYour health has fallen to zero. You collapse in the dungeon...");
                    Console.WriteLine("GAME OVER");
                    playing = false;
                }
            }
        }

        private void LookAround()
        {
            Console.WriteLine("\n" + currentRoom.GetDescription());

            // Display items
            List<string> items = currentRoom.GetItems();
            if (items.Count > 0)
            {
                Console.WriteLine("You see the following items:");
                foreach (string item in items)
                {
                    Console.WriteLine("- " + item);
                }
            }
            else
            {
                Console.WriteLine("There are no items in this room.");
            }

            // Display creatures
            List<Creature> creatures = currentRoom.GetCreatures();
            if (creatures.Count > 0)
            {
                Console.WriteLine("You see the following creatures:");
                foreach (Creature creature in creatures)
                {
                    Console.WriteLine($"- {creature.Name} (Health: {creature.Health})");
                }
            }

            // Display exits
            Dictionary<string, string> exits = currentRoom.GetExits();
            Console.WriteLine("Exits:");
            foreach (var exit in exits)
            {
                Console.WriteLine($"- {exit.Key}");
            }
        }

        private void DisplayPlayerStatus()
        {
            Console.WriteLine($"\n--- Player Status ---");
            Console.WriteLine($"Name: {player.Name}");
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Inventory: {player.InventoryContents()}");
        }

        private void Move()
        {
            // Display available exits
            Dictionary<string, string> exits = currentRoom.GetExits();
            Console.WriteLine("Available directions:");
            foreach (var exit in exits)
            {
                Console.WriteLine($"- {exit.Key}");
            }

            Console.Write("Which direction would you like to go? ");
            string direction = Console.ReadLine().ToLower();

            if (exits.ContainsKey(direction))
            {
                string destinationId = exits[direction];
                currentRoom = rooms[destinationId];
                Console.WriteLine($"You move {direction}.");
                Console.WriteLine(currentRoom.GetDescription());
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }

        private void TryPickUpItem()
        {
            List<string> items = currentRoom.GetItems();

            if (items.Count == 0)
            {
                Console.WriteLine("There are no items to pick up in this room.");
                return;
            }

            Console.WriteLine("Available items:");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"- {items[i]}");
            }

            Console.Write("What item would you like to pick up? ");
            string itemToPickUp = Console.ReadLine().ToLower();

            if (currentRoom.HasItem(itemToPickUp))
            {
                player.PickUpItem(itemToPickUp);
                currentRoom.RemoveItem(itemToPickUp);
                Console.WriteLine($"You picked up the {itemToPickUp}.");

                // Special message for the treasure
                if (itemToPickUp == "golden chalice")
                {
                    treasureFound = true;
                    Console.WriteLine("You found the treasure! Now find the exit to win!");
                }
            }
            else
            {
                Console.WriteLine($"There is no {itemToPickUp} here.");
            }
        }

        private void BattleCreature()
        {
            List<Creature> creatures = currentRoom.GetCreatures();

            if (creatures.Count == 0)
            {
                Console.WriteLine("There are no creatures to battle in this room.");
                return;
            }

            Console.WriteLine("Creatures in this room:");
            for (int i = 0; i < creatures.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {creatures[i].Name} (Health: {creatures[i].Health})");
            }

            Console.Write("Which creature would you like to battle? (enter number) ");
            if (!int.TryParse(Console.ReadLine(), out int creatureIndex) || creatureIndex < 1 || creatureIndex > creatures.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Creature target = creatures[creatureIndex - 1];
            Console.WriteLine($"You attack the {target.Name}!");

            // Simple battle mechanics
            Random rand = new Random();
            int playerDamage = rand.Next(5, 15);

            target.TakeDamage(playerDamage);
            Console.WriteLine($"You deal {playerDamage} damage to the {target.Name}!");

            if (target.Health <= 0)
            {
                Console.WriteLine($"You defeated the {target.Name}!");
                currentRoom.RemoveCreature(target);
            }
            else
            {
                int creatureDamage = target.Attack();
                player.TakeDamage(creatureDamage);
                Console.WriteLine($"The {target.Name} attacks you for {creatureDamage} damage!");
                Console.WriteLine($"Your health: {player.Health}");
            }
        }
    }
}