using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private int _hitCount;
        private int _misses;
        private int _timerCounter;
        private int _accuracy;
        public Standard()
        {
            InitializeComponent();
            _random = new Random();
            _targetCount = 0;
            StartSpawningTargets();
            //set file as resource first 
            this.Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Crosshair.cur")).Stream);

        }

        private void StartSpawningTargets()
        {
            // Set up a timer to spawn targets at random intervals
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (sender, args) =>
            //{
            //    SetTargets();
            //};
            timer.Tick += Tick;
            timer.Start();
        }
        private void Tick(object sender, EventArgs args)
        {
            SetTargets();
            lbl_timer.Content = _timerCounter;
            _timerCounter++;
        }
        private void SetTargets()
        {
            Ellipse target = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            double x = _random.NextDouble() * (canvas.ActualWidth - target.Width);
            double y = _random.NextDouble() * (canvas.ActualHeight - target.Height);
            

            Canvas.SetLeft(target, x);
            Canvas.SetTop(target, y);
            target.MouseLeftButtonDown += Target_MouseLeftButtonDown;

            canvas.Children.Add(target);
            RemoveTarget();
         
            tbl_hits.Text = _hitCount.ToString();
            tbl_misses.Text = _misses.ToString();

            _accuracy = (_hitCount / (_hitCount + _misses)) * 100;
            tbl_accuracy.Text = _accuracy.ToString();

        }
        private void RemoveTarget()
        {
            if (_targetCount > 10)
            {
                canvas.Children.RemoveAt(0);
                _targetCount--;
                _misses++;

            }
            _targetCount++;
        }
        private void Target_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;
            if (target != null)
            {
                canvas.Children.Remove(target);
                _targetCount--;
                _hitCount++;

            }
        
        }

    }
}
