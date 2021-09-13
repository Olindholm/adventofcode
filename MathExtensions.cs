namespace System {
    public static class MathExtensions {
        public static double PositiveModulo(double n, double m) {
            return (n % m + m) % m;
        }
        
        public static int PositiveModulo(int n, int m) {
            return (n % m + m) % m;
        }
    }
}
