using System.Collections.ObjectModel;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using witchmateCSharp.Models.Functions;
using witchmateCSharp.Models.Services;
using witchmateCSharp.ViewModels.Commands;

namespace witchmateCSharp.ViewModels;

public class MainViewModel : ViewModel
{
    private readonly ApproximationService _approximationService = new();
    private int _degree;
    private readonly Dictionary<float, float> _valuesDictionary;

    #region Bindings

    private string _a = string.Empty;
    private string _x = string.Empty;
    private string _y = string.Empty;
    private Visibility _degreeVisibility = Visibility.Visible;
    private string _currentMethod = string.Empty;
    private string _textDegree;
    private PlotModel _plot;
    private ObservableCollection<KeyValuePair<ApproximationMethod, string>> _methodsNames;
    private ObservableCollection<KeyValuePair<float, float>> _values;
    private ObservableCollection<Tuple<int, float>> _coefficients;

    public Visibility DegreeVisibility
    {
        get => _degreeVisibility;
        private set => SetField(ref _degreeVisibility, value);
    }

    public string Degree
    {
        get => _textDegree;
        set
        {
            if (SetField(ref _textDegree, value) && int.TryParse(_textDegree, out var degree))
            {
                if (degree >= 0)
                    _degree = degree;
                if (_methodsNames.FirstOrDefault(kv => kv.Value == CurrentMethod).Key == ApproximationMethod.LeastSquares)
                {
                    ChooseTypeMethod(ApproximationMethod.LeastSquares);
                }
            }
        }
    }

    public PlotModel Plot
    {
        get => _plot;
        private set => SetField(ref _plot, value);
    }

    public string X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    public string A
    {
        get => _a;
        set => SetField(ref _a, value);
    }

    public string Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    public string CurrentMethod
    {
        get => _currentMethod;
        private set => SetField(ref _currentMethod, value);
    }

    public ObservableCollection<KeyValuePair<ApproximationMethod, string>> MethodsNames
    {
        get => _methodsNames;
        private set => SetField(ref _methodsNames, value);
    }

    public ObservableCollection<Tuple<int, float>> Coefficients
    {
        get => _coefficients;
        private set => SetField(ref _coefficients, value);
    }

    public ObservableCollection<KeyValuePair<float, float>> Values
    {
        get => _values;
        private set => SetField(ref _values, value);
    }

    #endregion

    #region Commands

    public RelayCommand ChooseType => new(ChooseTypeMethod);

    private void ChooseTypeMethod(object parameter)
    {
        IFunctionSolution solution = new FunctionSolution([.._values]);


        if (parameter is ApproximationMethod method)
        {
            DegreeVisibility = method == ApproximationMethod.LeastSquares ? Visibility.Visible : Visibility.Collapsed;
            string name;
            switch (method)
            {
                case ApproximationMethod.LeastSquares:
                    MakeLeastSquares(solution, _degree);
                    name = _methodsNames.First(item => item.Key == ApproximationMethod.LeastSquares).Value;
                    CurrentMethod = name;

                    break;
                case ApproximationMethod.Lagrange:
                    MakeLagrange(solution);
                    name = _methodsNames.First(item => item.Key == ApproximationMethod.Lagrange).Value;
                    CurrentMethod = name;
                    break;
                case ApproximationMethod.Newton:
                    MakeNewton(solution);
                    name = _methodsNames.First(item => item.Key == ApproximationMethod.Lagrange).Value;
                    CurrentMethod = name;
                    break;
            }
        }
    }

    public RelayCommand DeletePoint => new(DeletePointMethod);

    private void DeletePointMethod(object parameter)
    {
        if (parameter is float x && _valuesDictionary.ContainsKey(x))
        {
            _valuesDictionary.Remove(x);
            Values = new(_valuesDictionary.ToList());

            var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

            ChooseTypeMethod(key);
        }
    }

    public RelayCommand AddPoint => new(AddPointMethod);

    private void AddPointMethod(object? parameter)
    {
        if (float.TryParse(X, out var x) && float.TryParse(Y, out var y))
        {
            if (_valuesDictionary.ContainsKey(x)) return;
            _valuesDictionary.Add(x, y);
            var xs = (from i in _valuesDictionary select i.Key).ToList();
            xs.Sort();

            List<KeyValuePair<float, float>> list = new();
            foreach (var t in xs)
            {
                list.Add(new(t, _valuesDictionary[t]));
            }

            Values = new(list);
            X = string.Empty;
            Y = string.Empty;

            var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

            ChooseTypeMethod(key);
        }
    }

    public RelayCommand AddCoefficient => new(AddCoefficientMethod);

    private void AddCoefficientMethod(object? parameter)
    {
        if (float.TryParse(_a, out var value))
        {
            _coefficients.Add(new(_coefficients.Count, value));
            A = string.Empty;
            var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

            ChooseTypeMethod(key);
        }
    }

    public RelayCommand DeleteCoefficient => new(DeleteCoefficientMethod);

    private void DeleteCoefficientMethod(object? parameter)
    {
        if (parameter is int index)
        {
            if (_coefficients.FirstOrDefault(tuple => tuple.Item1 == index) is { } item)
            {
                _coefficients.Remove(item);

                index = 0;
                List<Tuple<int, float>> list = [];
                foreach (var tuple in _coefficients)
                    list.Add(new(index++, tuple.Item2));

                Coefficients = new(list);
                var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

                ChooseTypeMethod(key);
            }
        }
    }

    public RelayCommand UpCoefficient => new(UpCoefficientMethod);

    private void UpCoefficientMethod(object? parameter)
    {
        if (parameter is int index && index != 0)
        {
            List<Tuple<int, float>> list = [.._coefficients];

            (list[index - 1], list[index]) =
                (new Tuple<int, float>(index - 1, _coefficients[index].Item2),
                    new Tuple<int, float>(index, _coefficients[index - 1].Item2));

            Coefficients = new(list);
            var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

            ChooseTypeMethod(key);
        }
    }

    public RelayCommand DownCoefficient => new(DownCoefficientMethod);

    private void DownCoefficientMethod(object? parameter)
    {
        if (parameter is int index && index != _coefficients.Count - 1)
        {
            List<Tuple<int, float>> list = [.._coefficients];

            (list[index + 1], list[index]) =
                (new Tuple<int, float>(index + 1, _coefficients[index].Item2),
                    new Tuple<int, float>(index, _coefficients[index + 1].Item2));

            Coefficients = new(list);
            var key = _methodsNames.First(kv => kv.Value == CurrentMethod).Key;

            ChooseTypeMethod(key);
        }
    }

    #endregion

    public MainViewModel()
    {
        MethodsNames =
        [
            new(ApproximationMethod.LeastSquares, "Метод наименьших квадратов"),
            new(ApproximationMethod.Lagrange, "Интерполяционный многочлен Лагранжа"),
            new(ApproximationMethod.Newton, "Интерполяционный многочлен Ньютона"),
        ];

        _valuesDictionary = new()
        {
            { -1, -1 }, { 0, 2 }, { 2, 10 }, { 3, 10 }, { 4, 15 }
        };
        Values = new(_valuesDictionary.ToList());
        Coefficients =
        [
            new(0, 2f),
            new(1, 349f / 60f),
            new(2, 41f / 40f),
            new(3, -91f / 60f),
            new(4, 11f / 40f),
        ];
        
        Degree = "0";
        ChooseTypeMethod(ApproximationMethod.LeastSquares);
    }

    private void MakeLeastSquares(IFunctionSolution solution, int degree)
    {
        var a = _approximationService.GetLeastSquaresMethodCoefficients(solution, degree);

        if (a is null)
        {
            MessageBox.Show("Не удалось рассчитать матрицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            Degree = "0";
            return;
        }

        PlotModel model = new();

        FunctionSeries functionSeries = new(PFunc, solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2,
            "Метод наименьших квадратов");

        MakePlot(solution, functionSeries);
        return;

        double PFunc(double x) => a.Select((value, i) => value * (float)Math.Pow(x, i)).Sum();
    }

    private void MakeLagrange(IFunctionSolution solution)
    {
        PlotModel model = new();

        FunctionSeries functionSeries = new((x) => _approximationService.CalculateLagrangeFunction(solution, (float)x),
            solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2, "Интерполяционный многочлен Лагранжа");

        MakePlot(solution, functionSeries);
    }

    private void MakeNewton(IFunctionSolution solution)
    {
        PlotModel model = new();

        var dividedDiff = _approximationService.GetNewtonDividedDifference(solution);

        FunctionSeries functionSeries = new(
            (x) => _approximationService.CalculateNewtonFunction(dividedDiff, solution, (float)x),
            solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2, "Интерполяционный многочлен Ньютона");

        MakePlot(solution, functionSeries);
    }

    private void MakePlot(IFunctionSolution solution, FunctionSeries functionSeries)
    {
        PlotModel model = new();

        FunctionSeries points = new();

        foreach (var item in solution.Solves)
            points.Points.Add(new DataPoint(item.Key, item.Value));

        points.Title = "Изначальные точки";
        points.MarkerType = MarkerType.Circle;
        points.MarkerSize = 4;
        points.Color = OxyColor.FromRgb(147, 0, 52);
        points.LineStyle = LineStyle.None;

        functionSeries.Color = OxyColor.FromRgb(239, 89, 118);

        model.Series.Add(functionSeries);


        if (_coefficients.Count > 0)
        {
            double Function(double x) => _coefficients.Select((tuple) => tuple.Item2 * (float)Math.Pow(x, tuple.Item1)).Sum();

            FunctionSeries series = new(Function,
                solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2, "Интерполяционный многочлен 4 степени")
            {
                LineStyle = LineStyle.Dash,
                Color = OxyColor.FromRgb(255, 177, 67)
            };

            model.Series.Add(series);
        }

        model.Series.Add(points);

        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "f(x)" });

        model.Title = functionSeries.Title;
        model.Subtitle = "Сравнение заданных точек и получившегося графика";
        Plot = model;
    }
}