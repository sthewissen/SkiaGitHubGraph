using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace SkiaSharpGitHub
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Dictionary<DateTime, int> data = new Dictionary<DateTime, int>();

        SKPaint noContribPaint = new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColor.Parse("#EBEDF0") };
        SKPaint oneContribPaint = new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColor.Parse("#9BE9A8") };
        SKPaint twoContribPaint = new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColor.Parse("#40C463") };
        SKPaint threeContribPaint = new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColor.Parse("#30A14E") };
        SKPaint moreContribPaint = new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColor.Parse("#216E39") };
        SKPaint textPaint = new SKPaint() { TextSize = 32, Color = SKColor.Parse("#767676") };

        public MainPage()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            var dateFrom = DateTime.Now.Date;
            var dateTo = dateFrom.AddYears(-1).AddDays(-2).Date;
            var rand = new Random();

            for (int i = 0; i < (dateFrom - dateTo).Days; i++)
            {
                data.Add(dateFrom.AddDays(-i), rand.Next(0, 4));
            }
        }

        void SKCanvasView_PaintSurface(System.Object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var x = 120;
            var y = 120;
            var row = -1;
            var columns = 7;
            var itemHeight = 64;
            var itemWidth = 64;
            var margin = 24;
            var cornerRadius = 8;

            for (int i = 0; i < data.Count; i++)
            {
                var currentDate = DateTime.Now.Date.AddDays(-i);

                if (i % columns == 0)
                {
                    x = 120;
                    row += 1;
                    y = (itemHeight + ((itemHeight + margin) * row));
                }
                else
                {
                    x += itemHeight + margin;
                }

                canvas.DrawRoundRect(new SKRoundRect(new SKRect(x, y, x + itemHeight, y + itemWidth), cornerRadius), GetColor(data[currentDate]));

                if (row == 0 && (
                        currentDate.DayOfWeek == DayOfWeek.Monday ||
                        currentDate.DayOfWeek == DayOfWeek.Wednesday ||
                        currentDate.DayOfWeek == DayOfWeek.Friday
                    ))
                {
                    canvas.DrawText($"{currentDate:ddd}", x, 30, textPaint);
                }

                if (currentDate.Day == 1)
                {
                    canvas.DrawText($"{currentDate:MMM}", 0, y + (itemHeight / 2), textPaint);
                }
            }

            //canvasView.HeightRequest = y;
        }

        SKPaint GetColor(int contributionCount) => contributionCount switch
        {
            0 => noContribPaint,
            1 => oneContribPaint,
            2 => twoContribPaint,
            3 => threeContribPaint,
            _ => moreContribPaint,
        };
    }
}
