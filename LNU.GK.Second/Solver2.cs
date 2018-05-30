using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    public class Solver2
    {
        private readonly double curant;
        private readonly int tCount;
        private double[,] T;

        private double c;

        public Solver2(double curant, int tCount,int c)
        {
            this.curant = curant;
            this.tCount = tCount;
            this.c = c;
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
            var U = T[n, j] - curant * (T[n, j + 1] - T[n, j]);

            return U;
        }
        private double Correct1(int n, int j)
        {
            n = n - 1;
            var correctnp1 = Predict1(n + 1, j);
            var correctnp1jm1 = Predict1(n + 1, j - 1);
            var U = 0.5 * ((T[n, j] + correctnp1) - curant * (correctnp1 - correctnp1jm1));

            return U;
        }

        private double Predict2(int n, int j)
        {
            n = n - 1;
            var U = T[n, j == 0 ? 50 - 1 : j] - c * curant * (T[n, j + 1] - T[n, j == 0 ? 50 - 1 : j]);

            return U;
        }
        private double Correct2(int n, int j)
        {
            n = n - 1;
            var correctnp1 = Predict2(n + 1, j);
            var correctnp1jm1 = Predict2(n + 1, j - 1);
            var U = 0.5 * ((T[n, j == 0 ? 50 - 1 : j] + correctnp1) - c * curant * (correctnp1 - correctnp1jm1));

            return U;
        }

        public double[,] Solve(
            Func<double, double> boundary,
            Func<double, double> start,
            double a,
            double b,
            int n)
        {
            var h = (b - a) / n;
            FillStartT(boundary, start, a, b, n);

            for (int i = 1; i <= tCount; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    T[i, j] = Correct2(i,j);
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
