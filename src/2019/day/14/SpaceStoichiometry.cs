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

            // Part one
            var balance = new StoichiometryBalance();

            // Add one fuel in dept (assume we already consumed it)
            // Convert other materials to fill that dept
            // In the making, we'll make dept of other things
            // Which also gets filled with different kind of dept
            // until only dept of ORE remains (which is ok)
            balance.AddItem("FUEL", -1);
            ConvertToOre(recipes, balance);

            long requiredOreForFuel = -balance.GetQuantity("ORE");
            Console.WriteLine("The ore required to produce 1 fuel is: {0}", requiredOreForFuel);

            // Part two
            balance.Clear();
            long availableOre = 1_000_000_000_000; // One trillion
            long fuelBalance = 0;

            // Clear previous balance
            balance.Clear();
            balance.AddItem("ORE", availableOre);
            
            // Same as before
            // Add one fuel of dept, convert to ore.
            // Continue until ore dept exceeds one trillion.
            while (true) {
                // Debug
                //Console.WriteLine("Ore: {0,13}, Fuel: {1,8}", balance.GetQuantity("ORE"), fuelBalance);

                // Calculate the minimum how much fuel can be made with the (current) available ore
                var minPossibleFuel = (long) Math.Max(1, Math.Floor((double) balance.GetQuantity("ORE") / requiredOreForFuel));
                balance.AddItem("FUEL", -minPossibleFuel);
                ConvertToOre(recipes, balance);

                // If the ore depts exceeds one trillion, break
                if (balance.GetQuantity("ORE") < 0) break;

                // Else we have successfully made 1 FUEL
                // (thus add that to our balance)
                fuelBalance += minPossibleFuel;
            }

            Console.WriteLine("With one trillion ore, the amount of fuel that can be made is: {0}", fuelBalance);
        }

        public void ConvertToOre(Dictionary<string, StoichiometryRecipe> recipes, StoichiometryBalance balance) {
            while (true) {
                // Debug
                //Console.WriteLine(balance);

                // Extract the missing compounds (needed to somehow exhange through reactions)
                // This inclues, all compounds with negative balances (excluding ORE)
                // Because ORE is abundant (free).
                var missingItems = balance.GetNonAbundantItems().Where(item => item.GetQuantity() < 0);
                
                // If no missing compunds, all reactions are complete, break
                if (missingItems.Count() == 0) break;

                // Otherwise...
                var missingItem = missingItems.First();
                StoichiometryRecipe recipe = recipes[missingItem.GetCompound()];

                // Calculate the number of reacions needed to clear the missing compound
                int multiple = (int) Math.Ceiling((double) -missingItem.GetQuantity() / recipe.GetOutput().GetQuantity());

                balance.Exchange(recipe, multiple);
            }
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
