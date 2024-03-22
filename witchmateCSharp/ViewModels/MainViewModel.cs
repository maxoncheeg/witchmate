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
    private ApproximationService _approximationService = new();
    private int _degree;

    private Visibility _degreeVisibility;
    private string _currentMethod = string.Empty;
    private string _textFunction = string.Empty;
    private string _textDegree;
    private PlotModel _plot;
    private ObservableCollection<KeyValuePair<ApproximationMethod, string>> _methodsNames;

    #region Bindings

    public Visibility DegreeVisibility
    {
        get => _degreeVisibility;
        private set => SetField(ref _degreeVisibility, value);
    }
    
    public string Degree
    {
        get => _textDegree;
        set {
            if (SetField(ref _textDegree, value) && int.TryParse(_textDegree, out var degree))
            {
                if (degree >= 0)
                    _degree = degree;
                if (_methodsNames.First(kv => kv.Value == CurrentMethod).Key == ApproximationMethod.LeastSquares)
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

    public string TextFunction
    {
        get => _textFunction;
        private set => SetField(ref _textFunction, value);
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

    #endregion

    #region Commands

    public RelayCommand ChooseType => new(ChooseTypeMethod);

    private void ChooseTypeMethod(object parameter)
    {
        IFunctionSolution solution = new FunctionSolution([
            new(-1, -1),
            new(0, 2),
            new(2, 10),
            new(3, 10),
            new(4, 15),
        ]);

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
                default:
                    break;
            }
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

        ChooseTypeMethod(ApproximationMethod.LeastSquares);
        Degree = "1";
    }

    private void MakeLeastSquares(IFunctionSolution solution, int degree)
    {
        var a = _approximationService.GetLeastSquaresMethodCoefficients(solution, degree);
        a.Reverse();

        Func<double, double> function = (x) => a.Select((value, i) => value * (float)Math.Pow(x, i)).Sum();

        PlotModel model = new();

        FunctionSeries functionSeries = new(function, solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2,
            "Метод наименьших квадратов");
        FunctionSeries points = new();
        FunctionSeries ourPoints = new();

        foreach (var item in solution.Solves)
        {
            points.Points.Add(new DataPoint(item.Key, item.Value));
            ourPoints.Points.Add(new DataPoint(item.Key, function(item.Key)));
        }

        points.Title = "Изначальные точки";
        points.MarkerType = MarkerType.Circle;
        points.MarkerSize = 4;
        points.LineStyle = LineStyle.None;

        ourPoints.Color = OxyColors.Transparent;
        ourPoints.SeriesGroupName = "Точки графика";
        ourPoints.MarkerType = MarkerType.Diamond;
        ourPoints.MarkerSize = 6;
        ourPoints.LineStyle = LineStyle.None;


        model.Series.Add(functionSeries);
        model.Series.Add(points);
        model.Series.Add(ourPoints);

        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "f(x)" });

        model.Title = "Метод наименьших квадратов";
        model.Subtitle = "Сравнение заданных точек и получившегося графика";
        Plot = model;
    }

    private void MakeLagrange(IFunctionSolution solution)
    {
        PlotModel model = new();

        FunctionSeries functionSeries = new((x) => _approximationService.CalculateLagrangeFunction(solution, (float)x),
            solution.Solves.First().Key, solution.Solves.Last().Key, 1e-2, "Интерполяционный многочлен Лагранжа");
        FunctionSeries points = new();

        foreach (var item in solution.Solves)
            points.Points.Add(new DataPoint(item.Key, item.Value));

        points.Title = "Изначальные точки";
        points.MarkerType = MarkerType.Circle;
        points.MarkerSize = 4;
        points.LineStyle = LineStyle.None;

        model.Series.Add(functionSeries);
        model.Series.Add(points);

        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "f(x)" });
        model.Title = "Интерполяционный член Лагранжа";
        model.Subtitle = "Сравнение заданных точек и получившегося графика";
        Plot = model;
    }

    private string MakeTextFunction(List<float> a)
    {
        string result = "f(x) = ";

        for (int i = a.Count - 1; i >= 0; i--)
        {
            result += $"{a[i]} * x^{i}";
            if (i != 0)
                result += " + ";
        }

        return result;
    }
}