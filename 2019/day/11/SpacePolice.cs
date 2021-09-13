using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SpacePolice : AdventOfCodePuzzle {
        public SpacePolice() : base(2019, 11) {}

        override public void Solve() {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());

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

            // Create grid
            EmergencyHullPaintingRobot robot = new EmergencyHullPaintingRobot();
            Dictionary<Point2D, int> spaceshipPanels = new Dictionary<Point2D, int>();

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

            // Finished program
            int paintedPanelsCount = spaceshipPanels.Count;

            Console.WriteLine(paintedPanelsCount);
        }
    }
}
