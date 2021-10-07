using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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

            int waldoOnes = waldoDigits[1];
            int waldoTwos = waldoDigits[2];
            int waldoNumber = waldoOnes * waldoTwos;

            Console.WriteLine("The layer with fewest 0 digits is: {0}", waldoLayer);
            Console.WriteLine("The number of 1 digits that layer contains is: {0}", waldoOnes);
            Console.WriteLine("The number of 2 digits that layer contains is: {0}", waldoTwos);
            Console.WriteLine("The product of number of 1 and 2 digits is: {0}", waldoNumber);


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

            Console.WriteLine("The decoded image is:");
            Console.Write(ImageToString(image));
        }

        public static string[] BLACK_AND_WHITE = {" ", "█"};
        public static string ImageToString(int[,] image, string[] colorPallette) {
            StringBuilder sb = new StringBuilder();

            int width = image.GetLength(0);
            int height = image.GetLength(1);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int color = image[x, y];
                    sb.Append(colorPallette[color]);
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public static string ImageToString(int[,] image) {
            return ImageToString(image, BLACK_AND_WHITE);
        }

        public static string ImageToString(IEnumerable<KeyValuePair<Point2D, int>> pixels, string[] colorPallette) {
            return ImageToString(PixelsToImage(pixels), colorPallette);
        }

        public static int[,] PixelsToImage(IEnumerable<KeyValuePair<Point2D, int>> pixels) {
            return PixelsToImage(pixels, false);
        }
        public static int[,] PixelsToImage(IEnumerable<KeyValuePair<Point2D, int>> pixels, bool flipY) {
            IEnumerable<int> xs = pixels.Select(pixel => pixel.Key.GetX());
            IEnumerable<int> ys = pixels.Select(pixel => pixel.Key.GetY());

            int xOffset = xs.Min();
            int yOffset = ys.Min();
            int width = 1 + xs.Max() - xOffset;
            int height = 1 + ys.Max() - yOffset;

            int[,] image = new int[width, height];
            foreach(var pixel in pixels) {
                Point2D p = pixel.Key;
                int color = pixel.Value;

                int x = p.GetX() - xOffset;
                int y = p.GetY() - yOffset;

                if (flipY) y = (height-1) - y; // Flip Y axis

                image[x, y] = color;
            }

            return image;
        }
    }
}
