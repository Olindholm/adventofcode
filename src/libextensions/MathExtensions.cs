using System.Linq;
using System.Numerics;

using static System.Math;

namespace System {
    public static class MathExtensions {
        public static double PositiveModulo(double n, double m) {
            return (n % m + m) % m;
        }

        public static int PositiveModulo(int n, int m) {
            return (n % m + m) % m;
        }

        public static long PositiveModulo(long n, long m) {
            return (n % m + m) % m;
        }
        public static BigInteger PositiveModulo(BigInteger n, BigInteger m) {
            return (n % m + m) % m;
        }

        public static BigInteger GCD(BigInteger firstFactor, params BigInteger[] otherFactors) {
            var answer = firstFactor;

            foreach (var factor in otherFactors) {
                var a = answer;
                var b = factor;

                while (b != 0) {
                    var temp = b;
                    b = a % b;
                    a = temp;
                }

                answer = a;
            }

            return answer;
        }

        public static bool IsCoprimes(BigInteger firstFactor, params BigInteger[] otherFactors) {
            return Abs((long) GCD(firstFactor, otherFactors)) == 1;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger m) {
            a = PositiveModulo(a, m);
            BigInteger i = m, v = 0, d = 1;

            while (a > 0) {
                BigInteger t = i/a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t*x;
                v = x;
            }

            return PositiveModulo(v, m);
        }

        public static long GCD(long firstFactor, params long[] otherFactors) {
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

                answer = (a / GCD(a, b)) * b;
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
        public static BigInteger Pow(BigInteger b, BigInteger exponent) {
            var result = new BigInteger(1);

            while (exponent > 0) {
                if ((exponent & 1) > 0) result = result * b;
                b = b * b;
                exponent = exponent >> 1;
            }

            return result;
        }
    }
}
