using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SpacePolice : AdventOfCodePuzzle {
        public SpacePolice() : base(2019, 11) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Paint the ship
            Dictionary<Point2D, int> spaceshipPanels = new Dictionary<Point2D, int>();
            PaintSpaceShip(program, spaceshipPanels);

            int paintedPanelsCount = spaceshipPanels.Count;
            Console.WriteLine("The number of panels painted (at least once) is: {0}", paintedPanelsCount);

            // Part Two
            // Clear previous paint
            spaceshipPanels.Clear();

            // Add white paint on origin
            spaceshipPanels[Point2D.ORIGIN] = 1;

            // Paint the ship again
            PaintSpaceShip(program, spaceshipPanels);

            // Convert spaceship panels to image
            int[,] image = SpaceImageFormat.PixelsToImage(spaceshipPanels, true);

            // Display image
            Console.WriteLine("The painted registration identifier should be 8 capital letters and looks as:");
            Console.Write(SpaceImageFormat.ImageToString(image));
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

                if (computer.IsFinished()) break;
            }

        }
    }
}
