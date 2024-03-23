using witchmateCSharp.Models.Functions;
using witchmateCSharp.Models.Matrix;

namespace witchmateCSharp.Models.Services;

public enum ApproximationMethod
{
    LeastSquares,
    Lagrange,
    Newton,
}

public class ApproximationService
{
    public List<float> GetLeastSquaresMethodCoefficients(IFunctionSolution solution, int degree)
    {
        List<float> cs = [];
        List<float> ds = [];

        for (int i = 0; i <= 2 * degree; i++)
        {
            float c = solution.Solves.Select((kv)
                => (float)Math.Pow(kv.Key, i)).Sum();

            cs.Add(c);
        }

        for (int i = 0; i <= degree; i++)
        {
            float d = solution.Solves.Select((kv)
                => kv.Value * (float)Math.Pow(kv.Key, i)).Sum();
            ds.Add(d);
        }

        List<List<float>> cMatrix = new();
        for (int i = 0; i <= degree; i++)
            cMatrix.Add(cs[i..(i + degree + 1)]);

        var a = Gauss.SolveMatrix(cMatrix, ds);

        return a;
    }

    public float CalculateLagrangeFunction(IFunctionSolution solution, float x)
    {
        float result = 0;

        for (int i = 0; i < solution.Solves.Count; i++)
        {
            float multiplication = 1;
            for (int j = 0; j < solution.Solves.Count; j++)
            {
                if (i == j) continue;
                multiplication *= (x - solution.Solves.ElementAt(j).Key)
                                  / (solution.Solves.ElementAt(i).Key - solution.Solves.ElementAt(j).Key);
            }

            result += solution.Solves.ElementAt(i).Value * multiplication;
        }

        return result;
    }

    public Dictionary<int, List<float>> GetNewtonDividedDifference(IFunctionSolution solution)
    {
        List<float> xs = solution.Solves.Select(pair => pair.Key).ToList();
        Dictionary<int, List<float>> steps = [];
        steps.Add(0, []);

        foreach (var pair in solution.Solves)
            steps[0].Add(pair.Value);

        int current = 0;
        do
        {
            steps.Add(current + 1, []);

            for (int i = 0; i < steps[current].Count - 1; i++)
            {
                float f = (steps[current][i] - steps[current][i + 1]) / (xs[i] - xs[i + current + 1]);
                steps[current + 1].Add(f);
            }
        } while (steps[++current].Count != 1);

        return steps;
    }

    public float CalculateNewtonFunction(Dictionary<int, List<float>> steps, IFunctionSolution solution, float x)
    {
        if (steps.Count != solution.Solves.Count) throw new ArgumentException();
        
        List<float> xs = solution.Solves.Select(pair => pair.Key).ToList();
        float result = steps[0][0];
        
        for (int i = 1; i < steps.Count; i++)
        {
            float multiple = 1;
            for (int j = 0; j < i; j++)
                multiple *= (x - xs[j]);
            multiple *= steps[i][0];
            result += multiple;
        }

        return result;
    }
}