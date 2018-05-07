using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> bound = (t) =>
            {
                if (t == 1) return 0;
                else return 1;
            };

            Func<double, double> start = (x) =>
            {
                if (-1 <= x && x <= 0) return 1;
                else return 0;
            };

            var tEnd = 100;

            var solver = new Solver(0.6, tEnd, 1);
            int n = 100;
            double a = -1;
            double b = 1;
            var step = (b - a) / n;
            var result = solver.Solve(bound, start, a, b, n);

            var left = new double[n+1];
            var right = new double[n+1];

            for (int i = 0; i <= n; i++)
            {
                left[i] = result[i, 0];
                right[i] = result[i, n];
            }


            StringBuilder script = new StringBuilder();
            script.AppendLine($"[X, Y] = meshgrid({a}:{step}:{b}, 0:{tEnd});");
            script.Append("U = [");
            for (int i = 0; i <= tEnd; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    var item = result[i, j];
                    script.Append($"{item} ");
                }
                if (i != tEnd)
                {
                    script.Append("; ");
                }
            }
            script.Append("];");

            script.AppendLine("figure");
            script.AppendLine("surf(X,Y,U);");
            var command = script.ToString();

            System.Console.ReadLine();
        }
    }
}
