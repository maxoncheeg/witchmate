#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <vector>

using namespace std;

#define FIRST
//#define SECOND

#ifdef FIRST
auto f_x = [](float x) -> float { return pow(x, 2) - 5 * x * sin(x) + x + 1; };
auto f_prime_x = [](float x) -> float { return 2 * x - 5 * sin(x) - 5 * x * cos(x) + 1; };
auto f_double_prime_x = [](float x) -> float { return 2 - 10 * cos(x) + 5 * x * sin(x); };
#endif // FIRST

#ifdef SECOND
auto f_x = [](float x) -> float { return 100 * pow(x, 2) - 10000 * x - 8; };
auto f_prime_x = [](float x) -> float { return 200 * x - 10000; };
auto f_double_prime_x = [](float x) -> float { return 200; };
#endif // DEBUG

auto phi = [](float x, float m) -> float { return x + 1.0 / m * f_x(x); };
auto phi_prime = [](float x, float m) -> float {return 1 + 1.0 / m * f_prime_x(x); };


#pragma region Newton
float getNewtonPrecision(float x, float precision) {
	float prevX;

	//if ((f_x(x) * f_double_prime_x(x) <= 0))
	//	throw new invalid_argument("x");

	int i = 0;

	do
	{
		prevX = x;

		printf("\t%d iteration: %.7f \n", i++, x);
		x = x - f_x(x) / f_prime_x(x);
	} while (abs(prevX - x) > precision);

	return x;
}

void printNewtonPrecisionValues(vector<float> xs, float precision) {
	clock_t start = clock();
	for (float x : xs) {
		printf("%.7f \n", getNewtonPrecision(x, precision));
	}
	clock_t end = clock();

	cout << "\nTIME: " << (end - start) << endl;

	cout << endl;
}

#pragma endregion

#pragma region SimpleIterations
float getSimpleIterationsValue(float x, float precision, float m,float q) {
	float prevX;
	int i = 0;

	float gPrecision = abs((q - 1) / q * precision);

	do {
		prevX = x;
		printf("\t%d iteration: %.7f \n", i++, x);
		x = phi(x, m);
	} while (abs(prevX - x) > (q < 0.5 ? precision : gPrecision));

	return x;
}

void printSimpleIterationsValue(vector<float> xs, float precision, vector<float> maxs, vector<float> phiMaxs) {
	if(maxs.size() != xs.size())throw new invalid_argument("xs, maxs sizes");

	clock_t start = clock();
	for (int i = 0; i < xs.size(); i++)
	{
		float q = phi_prime(phiMaxs[i], f_prime_x(maxs[i]) * -1);
		printf("%.7f \n", getSimpleIterationsValue(xs[i], precision, f_prime_x(maxs[i]) * -1, q));
	}
	clock_t end = clock();

	cout << "\nTIME: " << (end - start) << endl;
	cout << endl;
}
#pragma endregion

#pragma region Chord
float getChordValue(float a, float b, float precision) {
	float prevX;
	float x = 0;
	bool isB = false;
	int i = 0;

	if (isB = f_double_prime_x(b) * f_x(b) >= 0)
		x = a;
	else if (f_double_prime_x(a) * f_x(a) >= 0)
		x = b;
	else new invalid_argument("a and b");

	do {
		prevX = x;
		printf("\t%d iteration: %.7f \n", i++, x);

		if (isB)
			x = x - (b - x) * f_x(x) / (f_x(b) - f_x(x));
		else
			x = x - (x - a) * f_x(x) / (f_x(x) - f_x(a));
	} while (abs(prevX - x) > precision);


	return x;
}

void printChordValues(vector<pair<float, float>> intervals, float precision) {
	clock_t start = clock();
	for (pair<float, float> interval : intervals) {
		printf("%.7f \n", getChordValue(interval.first, interval.second, precision));
		
	}
	clock_t end = clock();

	cout << "\nTIME: " << (end - start) << endl;

	cout << endl;
}
#pragma endregion

int main() {
	setlocale(0, "RUS");


#ifdef FIRST

	vector<pair<float, float>> intervals = { make_pair(-3, -2), make_pair(-0.7, 0), make_pair(0.13, 0.93), make_pair(2, 3) };

	vector<float> calculatedXs = {};
	vector<float> xs = { -3, 0, 1, 3 };
	vector<float> maxs = { -3, -0.947, 0.946, 3 };
	vector<float> phiMaxs = { -2, -0.947, 0, 3 };
	 
#endif // FIRST


#ifdef SECOND
	vector<pair<float, float>> intervals = { make_pair(100, 101)};

	vector<float> calculatedXs = {};
	vector<float> xs = { 101 };
	vector<float> maxs = { 101 };
#endif // SECOND

	float precision = 1e-9;


	cout << endl << "Newton method:" << endl;
	printNewtonPrecisionValues(xs, precision);

	cout << endl << "Simple iterations method:" << endl;
	printSimpleIterationsValue(xs, precision, maxs, phiMaxs);

	cout << endl << "Chord method:" << endl;
	printChordValues(intervals, precision);

	getchar();
	getchar();
	return 0;
}