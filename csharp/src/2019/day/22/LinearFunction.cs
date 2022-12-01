using System.Numerics;

namespace AdventOfCode {
    public class LinearFunction {
        BigInteger A, B;

        public LinearFunction(BigInteger a, BigInteger b) {
            A = a;
            B = b;
        }

        public BigInteger Evaluate(BigInteger x) {
            return checked( A*x + B );
        }

        public BigInteger GetA() {
            return A;
        }

        public BigInteger GetB() {
            return B;
        }

        public LinearFunction Incorporate(LinearFunction that) {
            var a = this.GetA() * that.GetA();
            var b = this.GetA() * that.GetB() + this.GetB();
            return new LinearFunction(a, b);
        }
    }
}
