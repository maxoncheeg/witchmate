#include <iostream>
#include <vector>
#include <Windows.h>

#include "MatrixCalculateMethods.h"
using namespace std;

void firstQuest() {
    vector<vector<float>> A1 = {
        { 2.00001, 2, 3, 1 },
        { 1, 1, 1, 1 },
        { -1, 1, -2, 1 },
        { 2, 1, 1, 1}
    };

    auto A2 = vector<vector<float>>(A1);

    vector<float> B1 = { 8.00003, 2, -1, 5 };
    auto B2 = vector<float>(B1);

    chooseMainElements(A2);

    cout << "\t Matrix A1: " << endl;
    printMatrix(A1, B1);

    cout << "\t Matrix A2 with selection of major element by row: " << endl;
    printMatrix(A2, B2);

    convertToStepwise(A1, B1);
    convertToStepwise(A2, B2);

    cout << "\t Triangle matrix A1: " << endl;
    printMatrix(A1, B1);

    cout << "\t Triangle matrix A2: " << endl;
    printMatrix(A2, B2);

    auto x1 = findStepwiseXs(A1, B1);
    auto x2 = findStepwiseXs(A2, B2);

    cout << "\n\t Result A1: \t";
    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 12);
    for (float x : x1)
        cout << x << "\t\t";

    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 7);
    cout << "\n\t Result A2: \t";

    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 10);
    for (float x : x2)
        cout << x << "\t\t";

    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 7);
}

void secondQuest() {
    vector<vector<float>> A = {
        {10, 3, 1},
        {3, 14, 2},
        {-3, 2, 12}
    };

    vector<float> B = { 18, 35, 25 };

    float precision = 1e-7;

    printMatrix(A, B);

    convertToSimpleIterations(A, B);
    printMatrix(A, B);

    auto x = findSimpleIterationsXs(A, B, precision);
    for (int i = 0; i < x.size(); i++)
        cout << x[i] << "\t";
}

#pragma endregion

int main() {
    setlocale(LC_ALL, "");
    //firstQuest();
    secondQuest();

    return 0;
}
