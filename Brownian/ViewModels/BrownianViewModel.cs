using System.Collections.ObjectModel;
using Brownian.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Brownian.ViewModels;

public partial class BrownianViewModel : ObservableObject
{
    [ObservableProperty] private double _initialPrice = 100;
    [ObservableProperty] private double _sigma = 0.02;
    [ObservableProperty] private double _mean = 0.0005;
    [ObservableProperty] private int _numDays = 200;

    public ObservableCollection<double> Prices { get; } = new();

    [RelayCommand]
    private void Generate()
    {
        Prices.Clear();
        var result = BrownianModel.GenerateBrownianMotion(Sigma, Mean, InitialPrice, NumDays);
        foreach (var p in result)
            Prices.Add(p);
        OnPropertyChanged(nameof(Prices));
    }
}