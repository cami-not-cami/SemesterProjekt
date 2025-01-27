using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SemesterProjekt
{
    /// <summary>
    /// Interaktionslogik für Standard.xaml
    /// </summary>
    public partial class Standard : Window
    {
        private Random _random;
        private int _targetCount;
        public Standard()
        {
            InitializeComponent();
            _random = new Random();
            _targetCount = 0;
            StartSpawningTargets();
        }
     
        private void StartSpawningTargets()
        {
            // Set up a timer to spawn targets at random intervals
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1) // Spawns a target every second
            };
            timer.Tick += (sender, args) =>
            {
                SetTargets();
            };
            timer.Start();
        }
        private void SetTargets()
        {
            Ellipse target = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            double x = _random.NextDouble() * (canvas.ActualWidth - target.Width);
            double y = _random.NextDouble() * (canvas.ActualHeight - target.Height);

            Canvas.SetLeft(target, x);
            Canvas.SetTop(target, y);
            target.MouseLeftButtonDown += Target_MouseLeftButtonDown;


            canvas.Children.Add(target);
            if (_targetCount > 10)
            {
                canvas.Children.RemoveAt(0);
                _targetCount--;
            }
            _targetCount++;

        }
        private void Target_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;
            if (target != null)
            {
                {
                    canvas.Children.Remove(target);
                    _targetCount--;
                }

            }
        }

    }
}
