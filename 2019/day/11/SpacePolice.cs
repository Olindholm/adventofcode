using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SpacePolice : AdventOfCodePuzzle {
        public SpacePolice() : base(2019, 11) {}

        override public void Solve() {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());

            // Paint the ship
            Dictionary<Point2D, int> spaceshipPanels = new Dictionary<Point2D, int>();
            PaintSpaceShip(program, spaceshipPanels);

            int paintedPanelsCount = spaceshipPanels.Count;
            Console.WriteLine(paintedPanelsCount);

            // Part Two
            // Clear previous paint
            spaceshipPanels.Clear();

            // Add white paint on origin
            spaceshipPanels[Point2D.ORIGIN] = 1;

            // Paint the ship again
            PaintSpaceShip(program, spaceshipPanels);

            // Convert spaceship panels to image
            IEnumerable<int> xs = spaceshipPanels.Keys.Select(p => p.GetX());
            IEnumerable<int> ys = spaceshipPanels.Keys.Select(p => p.GetY());

            int xOffset = xs.Min();
            int yOffset = ys.Min();
            int width = 1 + xs.Max() - xOffset;
            int height = 1 + ys.Max() - yOffset;

            int[,] image = new int[width, height];
            foreach(KeyValuePair<Point2D, int> spaceshipPanel in spaceshipPanels) {
                Point2D p = spaceshipPanel.Key;
                int color = spaceshipPanel.Value;

                // For some reason, the registration code is written
                // upside down, thus flip Y-axis to display it properly
                int x = p.GetX() - xOffset;
                int y = (height-1) - (p.GetY() - yOffset); // Flip the image

                image[x, y] = color;
            }

            // Display image
            string[] colorPallette = {" ", "█"};
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int color = image[x, y];
                    Console.Write(colorPallette[color]);
                }
                Console.WriteLine("");
            }

        }

        public void PaintSpaceShip(long[] program, Dictionary<Point2D, int> spaceshipPanels) {
            
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

            // Load program
            computer.LoadProgram(program);

            // Init robot
            EmergencyHullPaintingRobot robot = new EmergencyHullPaintingRobot();

            // Paint the space ship
            while (true) {
                Point2D panelLocation = robot.GetLocation();
                
                // Retrieve color at robot's location, default to 0 (black)
                int panelColor = 0;
                spaceshipPanels.TryGetValue(panelLocation, out panelColor);

                // Run robot
                computer.Run(panelColor);

                // Retrieve robot output
                int paintColor = (int) computer.GetOutput();
                int turnDirection = (int) computer.GetOutput();

                // Paint color at robot's location
                spaceshipPanels[panelLocation] = paintColor;

                // Move the robot
                     if (turnDirection == 0) robot.TurnLeft();
                else if (turnDirection == 1) robot.TurnRight();
                else throw new Exception("Invalid turn!");
                robot.MoveForward(1);

                if (computer.isFinished()) break;
            }
            
        }
    }
}
