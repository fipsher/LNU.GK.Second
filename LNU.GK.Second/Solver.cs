﻿using System;
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

        private double Predict(int n, int j)
        {
            n = n - 1;
            var U = T[n, j] - c * curant * (T[n, j] - T[n, j - 1]);

            return U;
        }
        private double Correct(int n, int j)
        {
            n = n - 1;
            var correctnp1 = Predict(n + 1, j);
            var correctnp1jm1 = Predict(n + 1, j - 1);
            var U = 0.5 * ((T[n, j] + correctnp1) - c * curant * (correctnp1 - correctnp1jm1) - c * curant * (T[n, j] - 2 * T[n, j - 1] + T[n, j - 2]));

            return U;
        }

        private double Predict1(int n, int j)
        {
            n = n - 1;
            var U = T[n, j] - c * curant * (T[n, j + 1] - T[n, j]);

            return U;
        }
        private double Correct1(int n, int j)
        {
            n = n - 1;
            var correctnp1 = Predict1(n + 1, j);
            var correctnp1jm1 = Predict1(n + 1, j - 1);
            var U = 0.5 * ((T[n, j] + correctnp1) - c * curant * (correctnp1 - correctnp1jm1));

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

        private double Laks(int n, int j)
        {
            return 0.5 * (T[n, j + 1] + T[n, j - 1])
                - curant * 0.5 * (T[n, j + 1] - T[n, j - 1]);
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
                    T[i, j] = j == 1
                        ? Correct1(i, j)
                        : Correct(i, j);
                    // T[i, j] = Laks(i, j);
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
