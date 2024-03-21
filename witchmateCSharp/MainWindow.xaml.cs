using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using witchmateCSharp.Models.Functions;
using witchmateCSharp.Models.Services;

namespace witchmateCSharp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        IFunctionSolution solution = new FunctionSolution([
            new(-1, -1),
            new(0, 2),
            new(2, 10),
            new(3, 10),
            new(4, 15),
        ]);

        var x = 1;

        ApproximationService service = new();
        var a = service.GetLeastSquaresMethodCoefficients(solution, 3);

        string result = "| ";
        for (int i = 0; i < a.Count; i++)
        {
            result += $"a{i} = {a[i]} | ";
        }

        block.Text = result;
    }
}