using System.Collections.Immutable;
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
        TypeGrid.Visibility = _isTypesVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!char.IsDigit(e.Text, 0) && e.Text[0] != ',' && e.Text[0] != '-') e.Handled = true;
        else if (e.Text[0] == ',' && ((sender as TextBox)!).Text.IndexOf(',') > -1) e.Handled = true;
        else if (e.Text[0] == '-' && ((sender as TextBox)!).Text.Length > 0) e.Handled = true;
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is TextBox box)
        {
            int degree = int.Parse(box.Text);
            if (e.Delta > 0)
                degree++;
            else if (degree - 1 >= 0)
                degree--;

            box.Text = degree.ToString();
            ((DataContext as MainViewModel)!).Degree = box.Text;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (TextBoxA.IsFocused)
                ButtonA.Focus();
            else if (ButtonA.IsFocused)
                TextBoxA.Focus();

            else if (TextBoxX.IsFocused)
                TextBoxY.Focus();
            else if (TextBoxY.IsFocused)
                ButtonXY.Focus();
            else if (ButtonXY.IsFocused)
                TextBoxX.Focus();
        }
    }

    private void OnXYClick(object sender, RoutedEventArgs e)
    {
        TextBoxX.Focus();
    }
}