using Avalonia.Controls;
using Avalonia.Interactivity;
using modern_calculator_app.ViewModels;

namespace modern_calculator_app.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

}