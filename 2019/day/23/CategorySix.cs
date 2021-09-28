using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class CategorySix : AdventOfCodePuzzle {
        public CategorySix() : base(2019, 23) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Part one & two
            // First output is answer to part one
            // Last output is answer to part two
            RunNetwork(program);
        }

        void RunNetwork(long[] program) {
            // Init computers
            int numOfComputers = 50;
            var computers = new IntcodeComputer[numOfComputers];

            for (int i = 0; i < numOfComputers; i++) {
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

                computer.LoadProgram(program);
                computers[i] = computer;
            }

            // Init package queues
            var packageQueues = new Queue<long>[numOfComputers];
            for (int i = 0; i < numOfComputers; i++) packageQueues[i] = new Queue<long>();

            // Init NAT
            Point2D nat = null;
            var natMessages = new HashSet<Point2D>(); // Messages SENT from the NAT

            // Start all computers
            for (int i = 0; i < numOfComputers; i++) {
                computers[i].AddInput(i);
            }

            while (true) {
                var idle = true;

                for (int i = 0; i < numOfComputers; i++) {
                    IntcodeComputer computer = computers[i];
                    computer.Run(-1);

                    while (computer.HasMoreOutput()) {
                        idle = false;

                        long address = computer.GetOutput();
                        long X = computer.GetOutput();
                        long Y = computer.GetOutput();

                        if (address == 255) {
                            Console.WriteLine("Package to {0,2} [X={1}, Y={2}]", address, X, Y);
                            nat = new Point2D((int) X, (int) Y);
                        }
                        else {
                            computers[address].AddInput(X);
                            computers[address].AddInput(Y);
                        }
                    }
                }

                if (idle) {
                    long address = 0;
                    long X = nat.GetX();
                    long Y = nat.GetY();

                    Console.WriteLine("Package from NAT to {0,2} [X={1}, Y={2}]", address, X, Y);
                    computers[address].AddInput(nat.GetX());
                    computers[address].AddInput(nat.GetY());

                    if (!natMessages.Add(nat)) break;
                }
            }
        }
    }
}
