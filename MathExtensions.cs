using System.Linq;
using System.Collections.Generic;

namespace System {
    public static class MathExtensions {
        public static double PositiveModulo(double n, double m) {
            return (n % m + m) % m;
        }
        
        public static int PositiveModulo(int n, int m) {
            return (n % m + m) % m;
        }

        public static long GCF(long firstFactor, params long[] otherFactors) {
            long answer = firstFactor;

            foreach (var factor in otherFactors) {
                long a = answer;
                long b = factor;

                while (b != 0) {
                    long temp = b;
                    b = a % b;
                    a = temp;
                }

                answer = a;
            }

            return answer;
        }

        public static long LCM(long firstFactor, params long[] otherFactors) {
            long answer = firstFactor;

            foreach (var factor in otherFactors) {
                long a = answer;
                long b = factor;

                answer = (a / GCF(a, b)) * b;
            }

            return answer;
        }

        public static int SafeOverflowAdd(int a, int b) {
            int r = a + b;

            if (a > 0 && b > 0 && r < 0) return Int32.MaxValue;
            else if (a < 0 && b < 0 && r > 0) return Int32.MinValue;

            return r;
        }

        public static double Pythagoras(params double[] terms) {
            return Math.Sqrt(terms.Select(i => i*i).Sum());
        }
    }
}
