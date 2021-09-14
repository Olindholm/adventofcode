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
    }
}
