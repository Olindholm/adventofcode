using System;
using System.Numerics;

using static System.MathExtensions;
using static System.Numerics.BigInteger;

namespace AdventOfCode {
    public class BoundedLinearFunction {
        LinearFunction LinearFunction;
        BigInteger Bound;

        public BoundedLinearFunction(BigInteger a, BigInteger b, BigInteger bound) : this(new LinearFunction(a, b), bound) {}
        public BoundedLinearFunction(LinearFunction linearFunction, BigInteger bound) {
            LinearFunction = linearFunction;
            Bound = bound;
        }

        public BigInteger Evaluate(BigInteger x) {
            return PositiveModulo(LinearFunction.Evaluate(x), Bound);
        }

        public BigInteger EvaluateInverse(BigInteger y) {
            // Get rid of b
            // Let ax+b % m = y => ax % m = y-b
            var m = Bound;
            var u = PositiveModulo(y - GetB(), m);

            // If a m are coprimes, this is easy!
            var a = GetA();

            if (IsCoprimes(a, m)) {
                var aInverse = ModInverse(a, m);
                return PositiveModulo(aInverse * u, m);
            }
            else {
                BigInteger i = -1;
                BigInteger n;
                do {
                    i++;
                    n = (u + i * m);
                } while (n % a != 0);
                return n / a;
            }
        }

        public LinearFunction GetLinearFunction() {
            return LinearFunction;
        }

        public BigInteger GetA() {
            return LinearFunction.GetA();
        }

        public BigInteger GetB() {
            return LinearFunction.GetB();
        }
        public BigInteger GetBound() {
            return Bound;
        }

        public BoundedLinearFunction Incorporate(BoundedLinearFunction that) {
            if (this.GetBound() != that.GetBound())
                throw new Exception("Incorporating bounded linear functions must have same bounds!");

            var linearFunction = this.GetLinearFunction().Incorporate(that.GetLinearFunction());
            return new BoundedLinearFunction(PositiveModulo(linearFunction.GetA(), Bound), PositiveModulo(linearFunction.GetB(), Bound), Bound);
        }

        public BoundedLinearFunction IncorporateRecursively(long n) {
            Console.WriteLine("Bound: {0}", Bound);

            var oldA = GetA();
            var oldB = GetB();

            // Calculate new A
            var newA = ModPow(oldA, n, Bound);

            // Calulcate new B
            var den = 1 - oldA;
            BigInteger newB;

            if (IsCoprimes(den, Bound)) {
                den = ModInverse(den, Bound);
                var num = 1 - newA;
                newB = oldB * num * den;
            }
            else {
                var num = 1 - Pow(oldA, n);
                newB = oldB * num / den;
            }

            newB = PositiveModulo(newB, Bound);
            return new BoundedLinearFunction(newA, newB, Bound);
        }
    }
}
