﻿using witchmateCSharp.Models.Functions;
using witchmateCSharp.Models.Matrix;

namespace witchmateCSharp.Models.Services;

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
    
}