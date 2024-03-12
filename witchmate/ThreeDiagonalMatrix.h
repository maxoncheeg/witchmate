#pragma once
#include <vector>

using namespace std;

vector<float> findThreeDiagonalXs(vector<vector<float>> A, vector<float> B)
{
    vector<vector<float>> a(A.size(), vector<float>(A.size(), 0));
    vector<float> b(B.size(), 0);

    // check on three-diagonal-like matrix are needed

    for (int i = 0; i < A.size(); ++i)
    {
        if (i == 0)
        {
            a[i][i] = A[i][i]; //D1
            b[i] = B[i]; // B1
        }
        else
        {
            float alpha = a[i - 1][i] / a[i - 1][i - 1]; // Ei-1 / Di-1
            a[i][i] = A[i][i] - A[i][i - 1] * alpha; // Di - Ci * alpha

            float beta = b[i - 1] / a[i - 1][i - 1]; // Bi-1 / Di-1
            b[i] = B[i] - A[i][i - 1] * beta; // Bi - Ci * beta
        }

        if(i + 1 < a.size())
            a[i][i + 1] = A[i][i + 1]; // Ei
    }

    vector<float> xs(b.size(), 0);
    xs[xs.size() - 1] = b[b.size() - 1] / a[a.size() - 1][a.size() - 1];

    for (int i = xs.size() - 2; i >= 0; i--)
    {
        float alpha = a[i][i + 1] / a[i][i],
              beta = b[i] / a[i][i];// theres a problem here
        xs[i] = beta - alpha * xs[i + 1];
    }

    return xs;
}