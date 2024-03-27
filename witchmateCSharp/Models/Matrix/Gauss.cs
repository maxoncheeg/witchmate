namespace witchmateCSharp.Models.Matrix;

public static class Gauss
{
    public static List<float> SolveMatrix(List<List<float>> a, List<float> b)
    {
        List<int> indexes = [];
        for (int i = 0; i < a.Count; i++)
            indexes.Add(i);

        ChooseMainElements(a, indexes);
        ConvertToStepwise(a, b);
        return FindStepwiseXs(a, b, indexes);
    }

    private static void ChooseMainElements(List<List<float>> A, List<int> indexes)
    {
        for (int i = 0; i < A.Count; i++)
            for (int j = i + 1; j < A[i].Count; j++)
                if (Math.Abs(A[i][j]) > Math.Abs(A[i][i]))
                {
                    (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
                    foreach (var k in A)
                        (k[i], k[j]) = (k[j], k[i]);
                }
    }

    private static void ConvertToStepwise(List<List<float>> A, List<float> B)
    {
        for (int i = 0; i < A.Count; i++)
        {
            if (A.Count <= i || A[0].Count <= i) continue;

            float a = A[i][i];

            if (a == 0)
            {
                for (int k = i + 1; k < A.Count; k++)
                    if (A[k][i] != 0)
                    {
                        a = A[k][i];

                        (A[k], A[i]) = (A[i], A[k]);
                        (B[k], B[i]) = (B[i], B[k]);

                        break;
                    }

                if (a == 0) throw new ArgumentException("matrix with zeros");
            }

            for (int k = i + 1; k < A.Count; k++)
            {
                if (A[k][i] == 0) continue;

                float multiplier = A[k][i] / a;

                for (int j = 0; j < A[k].Count; j++)
                    A[k][j] -= A[i][j] * multiplier;

                B[k] -= B[i] * multiplier;
            }
        }
    }

    private static List<float> FindStepwiseXs(List<List<float>> A, List<float> B, List<int> indexes)
    {
        List<float> X = [];

        for (int i = A.Count - 1; i >= 0; i--)
        {
            float total = B[i];

            for (int j = A[i].Count - 1; j > i; j--)
                total -= X[A[i].Count - j - 1] * A[i][j];

            X.Add(total / A[i][i]);
        }

        X.Reverse();

        List<float> rightX = [];
        for (int i = 0; i < X.Count; i++)
            rightX.Add(X[indexes.IndexOf(i)]);

        return rightX;
    }
}