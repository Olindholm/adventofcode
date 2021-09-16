using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SpaceStoichiometry : AdventOfCodePuzzle {
        public SpaceStoichiometry() : base(2019, 14) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Extract recipes (chemical reactions)
            var recipes = new Dictionary<string, StoichiometryRecipe>();

            foreach (var recipeInstruction in puzzleInput.SplitToLines()) {
                // Decode recipe
                string[] recipeComponents = recipeInstruction.Split("=>");
                if (recipeComponents.Length != 2)  throw new Exception("Invalid recipe instruction");

                // Inputs
                string[] inputs = recipeComponents[0].Split(",");
                int numberOfInputs = inputs.Length;
                
                StoichiometryItem[] recipeInputs = new StoichiometryItem[numberOfInputs];
                for (int i = 0; i < numberOfInputs; i++) recipeInputs[i] = ParseReciptItem(inputs[i]);

                // Output
                var recipeOutput = ParseReciptItem(recipeComponents[1]);

                // Create recipe
                var recipe = new StoichiometryRecipe(recipeInputs, recipeOutput);
                recipes.Add(recipeOutput.GetCompound(), recipe);
            }

            // Alright!
            var balance = new StoichiometryBalance();
            balance.AddItem("FUEL", -1);

            while (true) {
                // Debug
                //Console.WriteLine(balance);

                // Extract the missing compounds (needed to somehow exhange through reactions)
                // This inclues, all compounds with negative balances (excluding ORE)
                // Because ORE is obundant (free).
                var missingItems = balance.GetItems().Where(item => !item.GetCompound().Equals("ORE")).Where(item => item.GetQuantity() < 0);
                
                // If no missing compunds, all reactions are complete, break
                if (missingItems.Count() == 0) break;

                // Otherwise...
                string missingCompound = missingItems.Select(item => item.GetCompound()).First();
                StoichiometryRecipe recipe = recipes[missingCompound];

                balance.Exchange(recipe);
            }

            int requiredOre = -balance.GetQuantity("ORE");
            Console.WriteLine("The ore required to produce 1 FUEL is: {0}", requiredOre);

        }

        StoichiometryItem ParseReciptItem(string recipeItem) {
            string[] recipeParts = recipeItem.Trim().Split(" ");
            if (recipeParts.Length != 2) throw new Exception("Invalid recipe item");

            int quantity = Int32.Parse(recipeParts[0]);
            string compound = recipeParts[1];

            return new StoichiometryItem(compound, quantity);
            
        }
    }
}
