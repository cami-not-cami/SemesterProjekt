using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Input;

namespace SemesterProjekt
{
    public class Target : Standard
    {
        private Random _random = new Random();
        private int _targetCount;
        private double _hitCount;
        private double _misses;
        private int _timerCounter = 1;
        private double _accuracy;
        private bool _isHit;
        private bool _checkedSize;
        private List<Ellipse> targetList = new List<Ellipse>();
        private DispatcherTimer timer;
        public Target(Canvas canvas, Label lbl_timer, TextBlock tbl_hits, TextBlock tbl_misses, TextBlock tbl_accuracy, CheckBox checkbox)
        : base(canvas, lbl_timer, tbl_hits, tbl_misses, tbl_accuracy, checkbox)
        {
            timer = new DispatcherTimer();
        }

        public void StartSpawningTargets()
        {
            // Set up a timer to spawn targets at random intervals
   
            timer.Interval = TimeSpan.FromSeconds(0.5);

            timer.Tick += Tick;
            timer.Start();
        }
        private void Tick(object sender, EventArgs args)
        {
            if (_timerCounter <= 60)
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
            double max = 60;

            Coordinates(target, _random);

            target.MouseLeftButtonDown += Target_MouseLeftButtonDown;
            canvas.Children.Add(target);



            //if its not checked we do standard random size
            if (checkbox.IsChecked == true)
            {
                DoubleAnimation resizing = new DoubleAnimation();
                resizing.From = target.Width - 10;
                resizing.To = max;
                resizing.Duration = TimeSpan.FromSeconds(1);
                resizing.AutoReverse = true;
                resizing.RepeatBehavior = RepeatBehavior.Forever;
                target.BeginAnimation(Ellipse.WidthProperty, resizing);
                target.BeginAnimation(Ellipse.HeightProperty, resizing);

            }
            _checkedSize = false;



            tbl_hits.Text = _hitCount.ToString();
            tbl_misses.Text = _misses.ToString();

            RemoveTarget();

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
        private void Coordinates(Ellipse target, Random random)
        {
            bool overlap = false;
            double xCoord = 0;
            double yCoord = 0;

            do
            {
                overlap = false;
                //coordinates random
                xCoord = _random.NextDouble() * (canvas.ActualWidth - target.Width);
                yCoord = _random.NextDouble() * (canvas.ActualHeight - target.Height);

                //gets coordinates of rectangle since my targets are placed by top and left not point aka radius
                Rect targetRect = new Rect(xCoord, yCoord, target.Width, target.Height);
                foreach (Ellipse tar in targetList)
                {
                    //gets the coordinates after it is placed
                    double placedX = Canvas.GetLeft(tar);
                    double placedY = Canvas.GetTop(tar);
                    //new coords of rectangle
                    Rect newRect = new Rect(placedX, placedY, tar.Width, tar.Height);

                    //if first rectangle intersects with the new one
                    if (targetRect.IntersectsWith(newRect))
                    {
                        overlap = true; break; //keeps trying until it doesnt overlap
                    }

                }
                //set targets on canvas
                Canvas.SetLeft(target, xCoord);
                Canvas.SetTop(target, yCoord);
                //add to list
                targetList.Add(target);


            } while (overlap);

        }
        private double AccuracyCalc()
        {
            double total = _hitCount + _misses;
            if (_hitCount > 0)
            {
                _accuracy = (_hitCount / total);
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
                targetList.RemoveAt(0);
                _targetCount--;
                _misses++;

            }
            _targetCount++;
        }
        public void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!_isHit)
            {
                _misses++;
            }
            _isHit = false;

        }
    }

}
