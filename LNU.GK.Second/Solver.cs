using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    public class Solver
    {
        private readonly double curant;
        private readonly int tCount;
        private double[,] T;

        private double c;

        public Solver(double curant, int tCount, int c)
        {
            this.curant = curant;
            this.tCount = tCount;
            this.c = c;
        }

        //private double Predict(int n, int j)
        //{
        //    n = n - 1;
        //    var U = T[n, j] - c * curant * (T[n, j] - T[n, j - 1]);

        //    return U;
        //}
        //private double Correct(int n, int j)
        //{
        //    n = n - 1;
        //    var correctnp1 = Predict(n + 1, j);
        //    var correctnp1jm1 = Predict(n + 1, j - 1);
        //    var U = 0.5 * ((T[n, j] + correctnp1) - c * curant * (correctnp1 - correctnp1jm1) - c * curant * (T[n, j] - 2 * T[n, j - 1] + T[n, j - 2]));

        //    return U;
        //}

        private double Predict(int n, int j)
        {
            n = n - 1;
            var U = T[n, j] - c * curant * (T[n, j+1] - T[n, j]);

            return U;
        }
        private double Correct(int n, int j)
        {
            n = n - 1;
            var correctnp1 = Predict(n + 1, j);
            var correctnp1jm1 = Predict(n + 1, j - 1);
            var U = 0.5 * ((T[n, j] + correctnp1) - c * curant * (correctnp1 - correctnp1jm1));

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
                T[i, 0] = boundary(a);
                T[i, n] = boundary(b);

                for (int j = 1; j < n; j++)
                {
                    //T[i, j] = j == 1 
                    //    ? Predict(i,j) 
                    //    : Correct(i, j);
                    T[i, j] = Correct(i, j);
                }
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
            T[0, 0] = boundary(a);
            T[0, n] = boundary(b);
        }
    }
}
