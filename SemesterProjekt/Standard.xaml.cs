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
        private int _targetCount ;
        private double _hitCount;
        private double _misses;
        private int _timerCounter =1;
        private double _accuracy;
        private bool _isHit;
        public Standard()
        {
            InitializeComponent();
            _random = new Random();
            _targetCount = 0;
            StartSpawningTargets();
            //set file as resource first 
            this.Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Crosshair.cur")).Stream);
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

        }
        private void StartSpawningTargets()
        {
            // Set up a timer to spawn targets at random intervals
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += Tick;
            timer.Start();
        }
        private void Tick(object sender, EventArgs args)
        {
            if (_timerCounter <=60)
            {
                SetTargets();
                lbl_timer.Content = _timerCounter;
                _timerCounter++;
            }
            else
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Stop();
                AccuracyCalc();
            }
          
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
          
            canvas.Children.Add(target);
            target.MouseLeftButtonDown += Target_MouseLeftButtonDown;
            
            RemoveTarget();
            tbl_hits.Text = _hitCount.ToString();
            tbl_misses.Text = _misses.ToString();


        }
        private double AccuracyCalc()
        {
            double total = _hitCount + _misses;
            if (_hitCount > 0)
            {
                _accuracy = (_hitCount / total) ;
            }
            //apparently % in a string actually makes it in a percentage
            tbl_accuracy.Text = _accuracy.ToString("0.##" + "%");


            return _accuracy;
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
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            if(!_isHit)
            {
                _misses++;
            }
          _isHit = false;

        }
        private void Target_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;

            if (target != null && canvas.Children.Contains(target))
            {
                _isHit = true;
                canvas.Children.Remove(target);
                _targetCount--;
                _hitCount++;
            }
           
        }

        private void ResizeTarget()
        {
            Ellipse target = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Random random = new Random(10);
            Width = target.Width + random.Next(1, 11);
            Height = target.Height + random.Next(1, 11);


        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
