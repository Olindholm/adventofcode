using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    
    class SecureContainer : AdventOfCodePuzzle {
        public SecureContainer() : base(2019, 4) {}
        
        override public void Solve() {
            int min = 152085;
            int max = 670283;
            int x;

            x = SecureContainer.call(min, max, false);
            Console.WriteLine("x = " + x);
            
            x = SecureContainer.call(min, max, true);
            Console.WriteLine("x = " + x);
        }


        public static int call(int min, int max, bool hardConstrain) {
            // It is assumed min < max
            int numDigits = (int) Math.Log10(max)+1;
            return asdf(min, max, numDigits, 0, hardConstrain);
        }
        public static bool hasAdjacentDigits(int x, int min, int max) {
            List<int> digitGroups = new List<int>();

            int previous = -1;
            while (x > 0) {
                int n = x % 10;
                if (n != previous) {
                    digitGroups.Add(0);
                    previous = n;
                }

                digitGroups[digitGroups.Count-1] += 1;
                x = x / 10;
            }

            foreach (int n in digitGroups) if (n >= min && n <= max) return true;
            return false;
        }
        
        public static int asdf(int min, int max, int n, int number, bool hardConstrain) {
            if (n < 0) {
                return (hasAdjacentDigits(number, 2, (hardConstrain) ? 2 : Int32.MaxValue)) ? 1 : 0;
            }

            int m = n+1;
            int mMin = (int) (min / Math.Pow(10, m));
            int mMax = (int) (max / Math.Pow(10, m));

            int bgn = (number > mMin) ? 0 : (int) (min / Math.Pow(10, n)) % 10;
            int end = (number < mMax) ? 9 : (int) (max / Math.Pow(10, n)) % 10;

            int nNumber = number % 10;
            bgn = Math.Max(bgn, nNumber);

            //Console.WriteLine("n = " + n + ", bgn = " + bgn + ", end = " + end + ", index = " + number);
            return (bgn > end) ? 0 : Enumerable.Range(bgn, 1+(end-bgn)).Select(i => asdf(min, max, n-1, number*10 + i, hardConstrain)).Sum();
        }
    }
}
