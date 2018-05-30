using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    public class Solver3
    {
        private readonly double curant;
        private readonly int tCount;
        private double[,] T;

        private double c;
        private double h;
        private readonly int m;

        public Solver3(double curant, int tCount, int c, int m)
        {
            this.curant = curant;
            this.tCount = tCount;
            this.c = c;
            this.m = m;
        }
        private double Step1(int n, int j)
        {
            return 0.5 * (T[n, j + 1] + T[n, j]) - 1d / 3d * curant * ((T[n, j + 1]) - (T[n, j]));
        }

        public double Step2(int n, int j)
        {
            return T[n, j] - (2d / 3d) * curant * ((Step1(n, j)) - (Step1(n, j - 1)));
        }

        public double GetU(int n, int j)
        {
            n--;
            var first = (1d / 24d) * curant * (-2 * T[n, j + 2] + 7 * (T[n, j + 1]) - 7 * (T[n, j - 1]) + 2 * (T[n, j - 2]));
            var second = (3d / 8d) * (curant) * ((Step2(n, j + 1)) - (Step2(n, j - 1)));
            return T[n, j] - first - second;
        }

        private double Predict1(int n, int j)
        {
            n = n - 1;
            var j1 = j == 0 ? 40 - 1 : j;

            var U = T[n, j1] - curant * (T[n, j + 1] - T[n, j1]);

            return U;
        }
        private double Correct1(int n, int j)
        {
            n = n - 1;
            var j1 = j == 0 ? 40 - 1 : j;
            var j1m1 = j - 1 == 0 ? 40 - 1 : j - 1;

            var correctnp1 = Predict1(n + 1, j);
            var correctnp1jm1 = Predict1(n + 1, j - 1);
            var U = 0.5 * ((T[n, j1] + correctnp1) - curant * (correctnp1 - correctnp1jm1));

            return U;
        }

        private double FCTS(int n, int j)
        {
            n = n - 1;
            var part1 = m * curant * ((T[n, j + 1] - 2 * T[n, j] + T[n, j - 1]) / h);
            var part2 = c * curant * (T[n, j + 1] - T[n, j - 1]) / 2d;

            return T[n, j] + part1 - part2;
        }

        public double[,] Solve(
            Func<double, double> boundary,
            Func<double, double> start,
            double a,
            double b,
            int n)
        {
            h = (b - a) / n;
            FillStartT(boundary, start, a, b, n);

            for (int i = 1; i <= tCount; i++)
            {
                //T[i, 0] = boundary(a);
                //T[i, n] = boundary(b);

                for (int j = 1; j < n; j++)
                {
                    T[i, j] = Correct1(i, j);
                    //T[i, j] = j ==1 || j == n-1 ? Correct1(i, j) : GetU(i,j);
                }
                T[i, 0] = (T[i, 1] + T[i, n]) / 2;
                T[i, n] = T[i, 0];
            }

            return (double[,])T.Clone();
        }

        private void FillStartT(Func<double, double> boundary, Func<double, double> start, double a, double b, int n)
        {
            T = new double[tCount + 1, n + 1];
            var h = (b - a) / n;
            for (int i = 0; i < n + 1; i++)
            {
                T[0, i] = start(a + h * i);
            }
            T[0, 0] = (T[0, 1] + T[0, n]) / 2;
            T[0, n] = T[0, 0];
        }
    }
}
