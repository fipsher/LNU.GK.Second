using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    static class First
    {
        public static void Do()
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
                if (-0.5 <= x && x <= 0.5) { return 1; }

                return 0;
            };

            var tEnd = 50;

            var solver = new Solver(1, tEnd, 1);
            int n = 50;
            double a = -1;
            double b = 1;
            var step = (b - a) / n;
            var result = solver.Solve(bound, start, a, b, n);

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


            for (int i = 0; i < result.GetLength(0); i+=9)
            {
                for (int j = 0; j < result.GetLength(1); j+=9)
                {
                    Console.Write($"[{i}, {j}]:{result[i,j].ToString("0.0000")} ");
                }
                    Console.WriteLine();
            }
        }
    }
}
