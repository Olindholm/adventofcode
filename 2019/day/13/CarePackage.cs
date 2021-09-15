using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class CarePackage : AdventOfCodePuzzle {
        public CarePackage() : base(2019, 13) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
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

            // Get graphics
            string[] colorPallette = {" ", "█", "#", "-", "o"};
            IEnumerable<KeyValuePair<Point2D, int>> tiles = retrieveTiles(computer);

            // Count blocks
            int numberOfBlocks = tiles.Where(tile => (tile.Value == 2)).Count();
            Console.WriteLine("The game has {0} block tiles and looks as:", numberOfBlocks);

            Console.Write(SpaceImageFormat.ImageToString(tiles, colorPallette));

            // Part two
            // Beat the game?
            Dictionary<Point2D, int> screen = new Dictionary<Point2D, int>();
            Point2D scorePosition = new Point2D(-1, 0);
            int score = 0;

            // Set the game free to play
            program[0] = 2;

            // Reload and run program
            computer.LoadProgram(program);
            computer.Run();

            while (true) {
                // Retrieve and draw graphics
                foreach (var tile in retrieveTiles(computer)) {
                    Point2D position = tile.Key;
                    if (position.Equals(scorePosition)) score = tile.Value;
                    else screen[position] = tile.Value;
                }
                //Console.Write(SpaceImageFormat.ImageToString(screen, colorPallette));

                // Poll user input
                //int input = GetUserInput();
                int input = GetAIInput(screen);

                // If computer is finished, break
                // (either by victory or loss)
                if (computer.IsFinished()) break;

                // Forward input and run computer
                computer.Run(input);
            }

            Console.WriteLine("The score after breaking the last block is: {0}", score);
        }

        int GetAIInput(IEnumerable<KeyValuePair<Point2D, int>> screen) {
            // Find ball, and horizontal padel
            Point2D paddle = screen.Where(tile => (tile.Value == 3)).Select(tile => tile.Key).First();
            Point2D ball = screen.Where(tile => (tile.Value == 4)).Select(tile => tile.Key).First();

            return Math.Sign(paddle.GetDeltaX(ball));
        }

        int GetUserInput() {
            while (true) {
                var key = Console.ReadKey(false).Key;

                switch(key) {
                    case ConsoleKey.DownArrow: return 0;
                    case ConsoleKey.LeftArrow: return -1;
                    case ConsoleKey.RightArrow: return 1;
                    case ConsoleKey.Escape:
                        System.Environment.Exit(1);
                        return 0; // Just return something
                }
            }
        }

        IEnumerable<KeyValuePair<Point2D, int>> retrieveTiles(IntcodeComputer computer) {
            Dictionary<Point2D, int> screen = new Dictionary<Point2D, int>();
            while (computer.HasMoreOutput()) {
                // Get 3 ouputs
                int x = (int) computer.GetOutput();
                int y = (int) computer.GetOutput();
                int c = (int) computer.GetOutput();

                screen[new Point2D(x, y)] = c;
            }

            return screen;
        }
    }
}
