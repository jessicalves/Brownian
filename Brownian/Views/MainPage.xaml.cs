using Brownian.ViewModels;
using SkiaSharp;

namespace Brownian.Views;

public partial class MainPage : ContentPage
{
    private readonly BrownianViewModel? _viewModel;

    public MainPage()
    {
        InitializeComponent();

        _viewModel = BindingContext as BrownianViewModel;

        if (_viewModel != null)
        {
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(BrownianViewModel.Prices))
                    ChartCanvas?.InvalidateSurface();
            };
        }
    }

    private void ChartCanvas_OnPaintSurface(object? sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        if (_viewModel == null || _viewModel.Prices.Count < 2)
            return;

        var prices = _viewModel.Prices.ToArray();
        double min = prices.Min();
        double max = prices.Max();
        if (Math.Abs(max - min) < 1e-12) max = min + 1;

        var info = e.Info;
        
        float left = 60;
        float right = info.Width - 20;
        float top = 20;
        float bottom = info.Height - 40;
        float plotWidth = right - left;
        float plotHeight = bottom - top;

        using var axisPaint = new SKPaint();
        axisPaint.Color = SKColors.Gray;
        axisPaint.StrokeWidth = 1;
        axisPaint.IsAntialias = true;
        axisPaint.Style = SKPaintStyle.Stroke;
        
        using var linePaint = new SKPaint();
        linePaint.Color = SKColors.Blue;
        linePaint.StrokeWidth = 3;
        linePaint.IsAntialias = true;
        linePaint.Style = SKPaintStyle.Stroke;

        canvas.DrawRect(left, top, plotWidth, plotHeight, axisPaint);
        
        using var path = new SKPath();
        for (int i = 0; i < prices.Length; i++)
        {
            float x = left + (i / (float)(prices.Length - 1)) * plotWidth;
            float norm = (float)((prices[i] - min) / (max - min));
            float y = bottom - norm * plotHeight;

            if (i == 0) path.MoveTo(x, y);
            else path.LineTo(x, y);
        }

        canvas.DrawPath(path, linePaint);
    }
}