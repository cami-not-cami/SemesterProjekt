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
using System.Reflection;

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
        private List<Point> targetPoints = new List<Point>();
        private bool _isCustomSet = false;
        private bool _Start = false;
        public double SizeSlider { get; set; } = 30.0;
        private int _customTargetIndex = 0;
        private DispatcherTimer timer;
        private List<Point> usedPositions = new List<Point>();


        private int _customTargetCounter = 0;
        public Target(Canvas canvas, Label lbl_timer, TextBlock tbl_hits, TextBlock tbl_misses, TextBlock tbl_accuracy, CheckBox checkbox)
        : base(canvas, lbl_timer, tbl_hits, tbl_misses, tbl_accuracy, checkbox)
        {
            timer = new DispatcherTimer();

        }
        

        public void StartSpawningTargets()
        {
            if (!_Start) return;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            //timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick -= Tick;
            timer.Tick += Tick;
            timer.Start();

        }
        private void Tick(object sender, EventArgs args)
        {

            if (_timerCounter <= 60)
            {
                canvas.IsEnabled = true;
                if (_isCustomSet)
                {

                    SetCustomTargets();
                }
                else
                {
                    SetTargets();
                }

                lbl_timer.Content = _timerCounter;
            }
            else
            {
                //timer.Stop();
                //canvas.IsEnabled = false;
                AccuracyCalc();
            }
            _timerCounter++;

        }
      
    
        private void SetCustomTargets()
        {
            if (_Start)
            {
                if (targetPoints.Count == 0)
                    return;
  
                Point position;
                position = targetPoints[_random.Next(targetPoints.Count)];
                int index = targetPoints.IndexOf(position);
                usedPositions.Add(position);

                //do
                //{
                //
                    
                //} while (usedPositions.Contains(position));
                
                Ellipse target = new Ellipse
                {
                    Width = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                    Height = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                    Fill = Brushes.Green,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
               
                target.MouseLeftButtonDown -= Target_MouseLeftButtonDown;
                target.MouseLeftButtonDown += Target_MouseLeftButtonDown;

                //if its not checked we do standard random size
                if (checkbox.IsChecked == true)
                {
                    DoubleAnimation resizing = new DoubleAnimation();
                    resizing.From = target.Width - 10;
                    resizing.To = 60;
                    resizing.Duration = TimeSpan.FromSeconds(1);
                    resizing.AutoReverse = true;
                    resizing.RepeatBehavior = RepeatBehavior.Forever;
                    target.BeginAnimation(Ellipse.WidthProperty, resizing);
                    target.BeginAnimation(Ellipse.HeightProperty, resizing);

                }
                _checkedSize = false;
                tbl_hits.Text = _hitCount.ToString();
                tbl_misses.Text = _misses.ToString();

                Canvas.SetLeft(target, position.X - target.Width / 2);
                Canvas.SetTop(target, position.Y - target.Height / 2);

                //if (position  == targetPoints[index] )
                //{
                //    canvas.Children.RemoveAt(index);
                //}
                if (_customTargetCounter >= targetPoints.Count)
                {

                    return;

                }
                else
                {
                    
                    _customTargetCounter++;
                    canvas.Children.Add(target);
                }
                RemoveTarget();

            }
        }
        public void SetTargets()
        {

            Ellipse target = new Ellipse
            {
                Width = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                Height = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            if (!Coordinates(target))
            {
                target = null;
                canvas.Children.Remove(target);
                return;
            }
            target.MouseLeftButtonDown -= Target_MouseLeftButtonDown;
            target.MouseLeftButtonDown += Target_MouseLeftButtonDown;
            canvas.Children.Add(target);

            //if its not checked we do standard random size
            if (checkbox.IsChecked == true)
            {

                DoubleAnimation resizing = new DoubleAnimation();
                resizing.From = SizeSlider;
                resizing.To = Math.Min(SizeSlider * 1.5, 68);
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
                _customTargetCounter--;

                _hitCount++;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns>Gibt an ob Koordinate im Deathloop haengt true alles ok false Deathloop</returns>
        private bool Coordinates(Ellipse target)
        {
            bool overlap = false;
            double xCoord = 0;
            double yCoord = 0;
            int watchdogCounter = 0;

            do
            {
                watchdogCounter++;
                overlap = false;

                if (!_isCustomSet)
                {
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
                }

                if (watchdogCounter > 50)

                {
                    return false;
                }

            } while (overlap);

            return true;
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
            while (canvas.Children.Count > 10)
            {
                canvas.Children.RemoveAt(0);
                _targetCount--;
                _misses++;

            }
            _targetCount++;
        }
        public void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_Start) return;
            if (!_isHit)
            {
                _misses++;

                tbl_misses.Text = _misses.ToString();
            }
            _isHit = false;
        }
        public void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_Start) return;
            _isCustomSet = true;

            Point clickedPoint = e.GetPosition(canvas);
            // Show a preview of the target
            Ellipse previewTarget = new Ellipse
            {
                Width = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                Height = double.IsRealNumber(SizeSlider) ? SizeSlider : 1,
                Fill = Brushes.Orange,
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };

            Canvas.SetLeft(previewTarget, clickedPoint.X - previewTarget.Width / 2);
            Canvas.SetTop(previewTarget, clickedPoint.Y - previewTarget.Height / 2);
          
            //inverts
            if (!CheckCustomCoordinates(clickedPoint))
            {
                previewTarget = null;
                return;
            }

            targetPoints.Add(clickedPoint);
            canvas.Children.Add(previewTarget);

        }
        private bool CheckCustomCoordinates(Point punkt)
        {

            if (targetPoints.Count == 0)
                return true;

            Rect temprect = new Rect(punkt.X, punkt.Y, SizeSlider, SizeSlider);

            foreach (var item in targetPoints)
            {
                // create a Rectangle around Point and check if intersects with Point
                if (temprect.IntersectsWith(new Rect(item.X, item.Y, SizeSlider - 10, SizeSlider - 10)))
                {
                    //overlap/no good
                    return false;
                }
            }
            //no overlap ok
            return true;
        }
        public void btn_start_Click(object sender, RoutedEventArgs e)
        {
            canvas.IsHitTestVisible = true;

            _Start = true;
            //canvas.IsEnabled = true;
            StartSpawningTargets();
        }
        public void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            _Start = false;
            _customTargetIndex = 0;
            _targetCount = 0;
            _hitCount = 0;
            _misses = 0;
            _accuracy = 0;
            _timerCounter = 0;
            timer.Stop();
            _customTargetCounter = 0;

            timer.Tick -= Tick;

            canvas.Children.Clear();
            targetList.Clear();

            if (_isCustomSet)
                targetPoints.Clear();

            tbl_hits.Text = string.Empty;
            tbl_accuracy.Text = string.Empty;
            tbl_misses.Text = string.Empty;
            lbl_timer.Content = string.Empty;

            canvas.IsHitTestVisible = true;

        }
        public void sld_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //triggers event that gets the new value from the ui as the user interacts with it
            //Dispatcher.InvokeAsync(() => SizeSlider = e.NewValue);
            SizeSlider = Math.Min(e.NewValue, 80);

        }

    }

}
