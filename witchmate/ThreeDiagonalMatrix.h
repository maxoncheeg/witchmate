#pragma once
#include <vector>

using namespace std;

vector<float> findThreeDiagonalXs(vector<vector<float>> A, vector<float> B)
{
    vector a(A.size(), vector<float>(A.size(), 0));
    vector<float> b(B.size(), 0);

    for (int i = 0; i < A.size(); ++i)
        for (int j = 0; j < A[i].size(); ++j)
            if (i == j && A[i][j] == 0)
                throw new invalid_argument("zeros!!!");
            else if (A[i][j] != 0 && abs(i - j) > 1)
                throw new invalid_argument("not zeros!!!");

    for (int i = 0; i < A.size(); ++i)
    {
        if (i == 0)
        {
            a[i][i] = A[i][i]; // D1
            b[i] = B[i]; // B1
        }
        else
        {
            float alpha = a[i - 1][i] / a[i - 1][i - 1]; // E_(i-1) / D_(i-1)
            a[i][i] = A[i][i] - A[i][i - 1] * alpha; // D_i - C_i * alpha

            float beta = b[i - 1] / a[i - 1][i - 1]; // B_(i-1) / D_(i-1)
            b[i] = B[i] - A[i][i - 1] * beta; // B_i - C_i * beta
        }

        if (i + 1 < a.size())
            a[i][i + 1] = A[i][i + 1]; // E_i
    }

    vector<float> xs(b.size(), 0);
    xs[xs.size() - 1] = b[b.size() - 1] / a[a.size() - 1][a.size() - 1];

    for (int i = xs.size() - 2; i >= 0; i--)
    {
        float alpha = a[i][i + 1] / a[i][i],
              beta = b[i] / a[i][i];
        xs[i] = beta - alpha * xs[i + 1];
    }

    return xs;
}
