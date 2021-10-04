using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static System.MathExtensions;

namespace AdventOfCode {
    class Cryostasis : AdventOfCodePuzzle {

        string[] MOVEMENT = { "north", "east", "south", "west" };
        public Cryostasis() : base(2019, 25) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Parse program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodeOutput());
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());
            computer.AddInstruction(new IntcodeAdjustRelativeBase());

            // Load and run program
            computer.LoadProgram(program);
            computer.Run();

            // Get initial room
            var room = ReindeerStarshipRoom.Parse(String.Concat(computer.GetAllOutput().Select(n => (char) n)));

            // Explore all rooms, and pick up all items
            room = ExploreMap(computer, room);

            // Find security check-point
            var direction = 0;
            while (!computer.IsFinished()) {
                if (room.GetName().Equals("Security Checkpoint")) break;

                direction = GetRightWallStepDirection(room, direction);
                room = Move(computer, direction);
            }

            // Get pressure sensitive floor direction
            direction = GetRightWallStepDirection(room, direction);

            // Get items
            string str = "";
            var allItems = GetInventory(computer).ToList();
            var inventory = allItems;

            for (var i = 1; i < allItems.Count; i++) {
                foreach (var combination in allItems.Combinations(i)) {

                    // Items to drop
                    foreach (var item in inventory.Except(combination)) computer.SendCommand("drop " + item);

                    // Items to pick up
                    foreach (var item in combination.Except(inventory)) computer.SendCommand("take " + item);

                    // Update the inventory
                    inventory = combination.ToList();
                    computer.Run();
                    str = String.Concat(computer.GetAllOutput().Select(n => (char) n));

                    // Move
                    computer.SendCommand(MOVEMENT[direction]);
                    computer.Run();

                    str = String.Concat(computer.GetAllOutput().Select(n => (char) n));
                    if (str.Contains("You may proceed.")) {
                        Console.WriteLine(str);
                        break;
                    }
                }
            }
        }

        int GetUserInput() {
            while (true) {
                var key = Console.ReadKey(true).Key;

                switch(key) {
                    case ConsoleKey.UpArrow: return 0;
                    case ConsoleKey.RightArrow: return 1;
                    case ConsoleKey.DownArrow: return 2;
                    case ConsoleKey.LeftArrow: return 3;
                    case ConsoleKey.Escape:
                        System.Environment.Exit(1);
                        return 0; // Just return something
                }

                Console.WriteLine(key);
            }
        }

        ReindeerStarshipRoom ExploreMap(IntcodeComputer computer, ReindeerStarshipRoom room) {
            // Init map
            var roomsVisits = new Dictionary<ReindeerStarshipRoom, int>();

            // Init
            int direction = 0;

            while (!computer.IsFinished()) {
                // Update the number of times we've
                // been in this room
                int previousVisits = roomsVisits.GetValueOrDefault(room, 0);
                roomsVisits[room] = previousVisits+1;

                // If we've been here at least 4 times
                // We've circled around and explored everything
                if (previousVisits >= 4) break;

                // Pick up possible items
                foreach (var item in room.GetItems()) computer.SendCommand("take " + item);

                // Otherwise...
                // Explore with right wall rule
                direction = GetRightWallStepDirection(room, direction);
                room = Move(computer, direction);
            }

            return room;
        }

        int GetRightWallStepDirection(ReindeerStarshipRoom room, int prevDirection) {
            var nextDirection = 0;

            for (int i = 0; i < 4; i++) {
                nextDirection = PositiveModulo(prevDirection+1-i, MOVEMENT.Length);
                if (room.HasDoorTo(MOVEMENT[nextDirection])) break;
            }

            return nextDirection;
        }

        ReindeerStarshipRoom Move(IntcodeComputer computer, int direction) {
            computer.SendCommand(MOVEMENT[direction]);
            computer.Run();
            return ReindeerStarshipRoom.Parse(String.Concat(computer.GetAllOutput().Select(n => (char) n)));
        }

        IEnumerable<string> GetInventory(IntcodeComputer computer) {
            computer.SendCommand("inv");
            computer.Run();

            string str = String.Concat(computer.GetAllOutput().Select(n => (char) n));
            var matches = Regex.Matches(str, "- ([A-z0-9 -]+)");
            return matches.Select(m => m.Groups[1].ToString());
        }
    }
}
