#pragma once
#include <iostream>
#include <vector>

using namespace std;


float getMatrixNorm(vector<vector<float>> a) {
	float sum = 0;
	for (int i = 0; i < a.size(); i++)
		for (int j = 0; j < a[i].size(); j++)
			sum += pow(a[i][j], 2);

	return sqrt(sum);
}

void printMatrix(vector<vector<float>>& A, vector<float>& B) {


	for (int i = 0; i < A.size(); i++)
	{
		for (int j = 0; j < A[i].size(); j++)
			printf("%.7f\t", A[i][j]);
		printf("|\t%.7f\n", B[i]);
	}
	cout << endl;
}

vector<float> substractVectors(vector<float> a, vector<float> b) {
	vector<float> x = {};

	for (int i = 0; i < a.size(); i++)
		x.push_back(a[i] - b[i]);

	return x;
}

float vectorLength(vector<float> x) {
	float sum = 0;
	for (float i : x)
		sum += pow(i, 2);
	return sqrt(sum);
}

#pragma region FIRST QUEST

// matrix should be square like shit maybe, so i think i do it right
// todo: fix this shit
void chooseMainElements(vector<vector<float>>& A) {
	for (size_t i = 0; i < A.size(); i++)
		for (size_t j = i + 1; j < A[i].size(); j++)
			if (abs(A[i][j]) > abs(A[i][i]))
			{
				for (auto& k : A)
				{
					float temp = k[i];
					k[i] = k[j];
					k[j] = temp;
				}

				continue;
			}
}

// tests are needed
void convertToStepwise(vector<vector<float>>& A, vector<float>& B) {
	for (int i = 0; i < A.size(); i++)
	{
		if (A.size() <= i || A[0].size() <= i) continue;

		float a = A[i][i];

		if (a == 0) {
			for (size_t k = i + 1; k < A.size(); k++)
				if (A[k][i] != 0)
				{
					a = A[k][i];

					A[k].swap(A[i]);

					auto tempB = B[k];
					B[k] = B[i];
					B[i] = tempB;

					break;
				}

			if (a == 0) continue;
		}

		for (int k = i + 1; k < A.size(); k++)
		{
			if (A[k][i] == 0) continue;

			float multiplier = A[k][i] / a;

			for (int j = 0; j < A[k].size(); j++)
				A[k][j] -= A[i][j] * multiplier;

			B[k] -= B[i] * multiplier;
		}
	}
}

vector<float> findStepwiseXs(vector<vector<float>>& A, vector<float>& B) {
	vector<float> X = {};

	for (int i = A.size() - 1; i >= 0; i--)
	{
		float total = B[i];
		for (int j = A[i].size() - 1; j >= 0; j--)
			if (A[i][j] != 0)
			{
				if (X.size() >= A[i].size() - j)
					total -= X[A[i].size() - j - 1] * A[i][j];
				else
					X.push_back(total / A[i][j]);
			}
			else break;
	}

	reverse(X.begin(), X.end());
	return X;
}

#pragma endregion

#pragma region SECOND QUEST

void convertToSimpleIterations(vector<vector<float>>& A, vector<float>& B) {
	for (int i = 0; i < A.size(); i++)
	{
		float sum = 0;
		for (int j = 0; j < A[i].size(); j++)
			if (j == i)continue;
			else sum += abs(A[i][j]);

		if (abs(A[i][i]) <= sum) throw new runtime_error("|A(ii)| < sum of |A(ij)|");
	}

	for (int i = 0; i < A.size(); i++)
	{
		float a = A[i][i];
		B[i] /= a;

		for (int j = 0; j < A.size(); j++)
			if (i == j) A[i][j] = 0;
			else A[i][j] /= -a;
	}
}

vector<float> findSimpleIterationsXs(vector<vector<float>>& a, vector<float>& b, float precision) {
	vector<float> x = b;
	int k = 0;

	float norm = getMatrixNorm(a);

	if (norm >= 1)
		throw new runtime_error("|alpha| > 1");

	if (norm > 0.5)
		precision = (1 - norm) / norm * precision;
	float error = precision;
	do {
		vector<float> xk = { 0, 0, 0 };
		for (int i = 0; i < x.size(); i++)
		{
			float sum = 0;
			for (int j = 0; j < a[i].size(); j++) {
				sum += a[i][j] * x[j];
			}

			xk[i] = b[i] + sum;
		}

		cout << "\t" << k << " iteration: ";
		for (int k = 0; k < xk.size(); k++) printf("%.7f ", xk[k]);
		cout << endl;

		error = vectorLength(substractVectors(xk, x));
		k++;
		x = vector<float>(xk);
	} while (error > precision);

	cout << "Iterations amount - " << k << endl;

	return x;
}

#pragma endregion