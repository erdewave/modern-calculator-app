using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace modern_calculator_app.ViewModels;

public partial class MainWindowViewModel : INotifyPropertyChanged
{
    
    private string _displayText = "0";
    private string _oldText;
    private double _firstNumber;
    private double _secondNumber;
    private string _operation = string.Empty;


    public string DisplayText
    {
        get { return _displayText; }
        set
        {
            _displayText = value;
            OnPropertyChanged();
        }
    }

    public string OldText
    {
        get => _oldText;
        set
        {
            if (value == _oldText) return;
            _oldText = value;
            OnPropertyChanged();
        }
    }

    public double FirstNumber
    {
        get => _firstNumber;
        set
        {
            if (value.Equals(_firstNumber)) return;
            _firstNumber = value;
            OnPropertyChanged();
        }
    }

    public double SecondNumber
    {
        get => _secondNumber;
        set
        {
            if (value.Equals(_secondNumber)) return;
            _secondNumber = value;
            OnPropertyChanged();
        }
    }

    public string Operation
    {
        get => _operation;
        set
        {
            if (value == _operation) return;
            _operation = value;
            OnPropertyChanged();
        }
    }

    public ICommand NumberCommand { get; }
    public ICommand OperationCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    
    
    public MainWindowViewModel()
    {
        NumberCommand = new RelayCommand<string>(OnNumberClicked!);
        EqualsCommand = new RelayCommand(OnEqualsClicked);
        OperationCommand = new RelayCommand<string>(OnOperationClicked!);
        ClearCommand = new RelayCommand(OnClearClicked);
    }
    
    private void OnNumberClicked(string number)
    {
        if (DisplayText == "0")
            DisplayText = number;        
        else if (DisplayText.Contains(Operation) && Operation != "")
            DisplayText += number;         
        else
            DisplayText += number;      
    }

    private void OnOperationClicked(string operation)
    {
        if (!string.IsNullOrEmpty(Operation) && DisplayText.EndsWith($" {Operation} "))
        {
            DisplayText = DisplayText.Replace($" {Operation} ", $" {operation} ");
            Operation = operation;
            return;
        }
        
        if (!string.IsNullOrEmpty(Operation) && !DisplayText.EndsWith($" {Operation} "))
        {
            string[] parcalar = DisplayText.Split(' ');
            if (parcalar.Length >= 3 && double.TryParse(parcalar[parcalar.Length - 1], out double secondNum))
            {
                SecondNumber = secondNum;
                double result = 0;
            
                if (Operation == "+") result = FirstNumber + SecondNumber;
                else if (Operation == "-") result = FirstNumber - SecondNumber;
                else if (Operation == "*") result = FirstNumber * SecondNumber;
                else if (Operation == "/") result = FirstNumber / SecondNumber;
            
                DisplayText = result.ToString();
                FirstNumber = result; 
            }
        }
        else
        {
            FirstNumber = double.Parse(DisplayText);
        }
    
        Operation = operation;
        DisplayText = DisplayText + " " + operation + " ";
    }

    private void OnClearClicked()
    {
        DisplayText = "0";
        OldText = String.Empty;
        FirstNumber = 0;
        SecondNumber = 0;
        Operation = string.Empty;
    }
    private void OnEqualsClicked()
    {
        string[] parcalar = DisplayText.Split(' ');
        int sonIndex = parcalar.Length - 1;
        
        if (string.IsNullOrWhiteSpace(parcalar[sonIndex]) || !double.TryParse(parcalar[sonIndex], out double secondNum))
        {
            return;  
        }
    
        SecondNumber = secondNum;
        
        double result = 0;
        if (Operation == "+")
        {
            result = FirstNumber + SecondNumber;
        }
        else if (Operation == "-")
        {
            result = FirstNumber - SecondNumber;
        }
        else if (Operation == "x")
        {
            result = FirstNumber * SecondNumber;
        }
        else if (Operation == "/")
        {
            result = FirstNumber / SecondNumber;
        }
        DisplayText = result.ToString();
        OldText =  FirstNumber + _operation +  SecondNumber;

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}