#include <iostream>
#include <vector>
#include <Windows.h>

#include "MatrixCalculateMethods.h"
#include "ThreeDiagonalMatrix.h"
using namespace std;

void firstQuest()
{
    vector<vector<float>> A1 = {
        {2.00001f, 2, 3, 1},
        {1, 1, 1, 1},
        {-1, 1, -2, 1},
        {2, 1, 1, 1}
    };

    

    auto A2 = vector(A1);

    vector<float> B1 = {8.00003f, 2, -1, 5};
    auto B2 = vector<float>(B1);

    cout << "\n\n" << translateToWolfram(A1, B1) << "\n\n";
    chooseMainElements(A2);

    cout << "\t Matrix A1: " << '\n';
    printMatrix(A1, B1);

    cout << "\t Matrix A2 with selection of major element by row: " << endl;
    printMatrix(A2, B2);

    convertToStepwise(A1, B1);
    convertToStepwise(A2, B2);

    cout << "\t Triangle matrix A1: " << endl;
    printMatrix(A1, B1);

    cout << "\t Triangle matrix A2: " << endl;
    printMatrix(A2, B2);

    const auto x1 = findStepwiseXs(A1, B1);
    const auto x2 = findStepwiseXs(A2, B2);

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

void secondQuest()
{
    vector<vector<float>> A = {
        {10, 3, 1},
        {3, 14, 2},
        {-3, 2, 12}
    };

    vector<float> B = {18, 35, 25};

    float precision = 1e-7;
    
    cout << "\n\n" << translateToWolfram(A, B) << "\n\n";

    printMatrix(A, B);

    convertToSimpleIterations(A, B);
    printMatrix(A, B);

    cout << "\t Simple iterations: \n";
    auto x = findSimpleIterationsXs(A, B, precision);
    for (auto i : x)
        printf("%.7f \t", i);

    cout << "\n\n\t Seidel: \n";
    x = findSeidelXs(A, B, precision);

    for (auto i : x)
        printf("%.7f \t", i);
}

void thirdQuest()
{
    vector<vector<float>> a = {
        {1, 2, 0, 0, 0, 0, 0},
        {3, 3, 2, 0, 0, 0, 0},
        {0, 4, 2, 3, 0, 0, 0},
        {0, 0, 4, 3, 3, 0, 0},
        {0, 0, 0, 2, 2, 3, 0},
        {0, 0, 0, 0, 1, 2, 5},
        {0, 0, 0, 0, 0, 2, 2},
    };

    vector<float> b = {1, 2, 3, 4, 5, 6, 7};

    cout << "\n\n" << translateToWolfram(a, b) << "\n\n";

    cout << "\tThree diagonal matrix:\n";
    printMatrix(a, b);


    auto xs = findThreeDiagonalXs(a, b);

    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 10);
    for (auto x : xs)
        printf("%.7f\t", x);
    cout << endl;

    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), 7);
}

int main()
{
    int choise = 0;

lol:
    cin >> choise;

    switch (choise)
    {
    case 1:
        firstQuest();
        break;
    case 2:
        secondQuest();
        break;
    case 3:
        thirdQuest();
        break;
    default:
        goto lol;
        break;
    }

    getchar();
    getchar();
    return 0;
}
