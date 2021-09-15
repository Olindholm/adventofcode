using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SpaceImageFormat : AdventOfCodePuzzle {
        public SpaceImageFormat() : base(2019, 8) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load
            string encodedImageData = puzzleInput;
            
            // imageLayers properties
            int width = 25;
            int height = 6;
            int area = width * height;
            int layers = encodedImageData.Length / area;

            // Extract layers
            int[,,] imageLayers = new int[layers, width, height];

            for (int i = 0; i < encodedImageData.Length; i++) {
                int layer = i / area;
                int x = i % area % width;
                int y = i % area / width;

                string pixel = encodedImageData[i].ToString();
                imageLayers[layer, x, y] = Int32.Parse(pixel);
            }

            // Find layer with fewest zeroes
            int waldoLayer = -1;
            int waldoZeros = Int32.MaxValue;

            for (int layer = 0; layer < layers; layer++) {
                int zeros = 0;
                for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) if (imageLayers[layer, x, y] == 0) zeros++;

                if (zeros < waldoZeros) {
                    waldoLayer = layer;
                    waldoZeros = zeros;
                }
            }

            int[] waldoDigits = new int[9];
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) {
                int pixelDigit = imageLayers[waldoLayer, x, y];
                waldoDigits[pixelDigit]++;
            }

            int answer = waldoDigits[1] * waldoDigits[2];
            Console.WriteLine(answer);

            // Part Two
            int transparent = 2;
            int[,] image = new int[width, height];
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) image[x, y] = transparent;
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) {
                for (int layer = 0; layer < layers; layer++) {
                    int color = imageLayers[layer, x, y];

                    if (color != transparent) {
                        image[x, y] = color;
                        break;
                    }
                }
            }
            
            string[] colorPallette = {" ", "█"};
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int color = image[x, y];
                    Console.Write(colorPallette[color]);
                }
                Console.WriteLine("");
            }
        }
    }
}
