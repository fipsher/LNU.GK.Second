using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.Second
{
    static class Third
    {
        public static void Do()
        {
            var L = 1;
            Func<double, double> bound = (t) =>
            {
                return Math.Sin(Math.PI * t / L);
            };
            Func<double, double> start = (x) =>
            {
                return Math.Sin(Math.PI * x / L);
            };

            var tEnd = 50;

            var solver = new Solver3(1, tEnd, 1, 1);
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

            for (int i = 0; i < result.GetLength(0); i += 9)
            {
                for (int j = 0; j < result.GetLength(1); j += 9)
                {
                    Console.Write($"[{i.ToString("00")}, {j.ToString("00")}]:{(result[i, j] > 0 ? result[i, j].ToString("00.0000") : result[i, j].ToString("0.0000"))} ");
                }
                Console.WriteLine();
            }
        }
    }
}
