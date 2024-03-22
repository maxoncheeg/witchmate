using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using witchmateCSharp.ViewModels;

namespace witchmateCSharp.Views;

public partial class MainWindow : Window
{
    private bool _isTypesVisible = true;
        
    public MainWindow()
    {
        InitializeComponent();
        ChangeTypesVisibility();
    }

    private void OnChangeTypesVisibility(object sender, RoutedEventArgs e)
    {
        ChangeTypesVisibility();
    }

    private void ChangeTypesVisibility()
    {
        _isTypesVisible = !_isTypesVisible;
        typeGrid.Visibility = _isTypesVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!char.IsDigit(e.Text, 0)) e.Handled = true;
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is TextBox box)
        {
            int degree = int.Parse(box.Text);
            if (e.Delta > 0)
            {
                degree++;
            }
            else
            {
                if(degree - 1 >= 0)
                    degree--;
            }

            box.Text = degree.ToString();
            (DataContext as MainViewModel).Degree = box.Text;
        }
    }
}