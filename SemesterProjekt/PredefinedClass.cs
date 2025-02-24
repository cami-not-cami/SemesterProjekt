using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SemesterProjekt
{
    internal class PredefinedClass : Target
    {
        public int _customTargetIndex = 0;
        private Random _rnd;
        private Level _level;


        public PredefinedClass(Canvas canvas, Label lbl_timer, TextBlock tbl_hits, TextBlock tbl_misses, TextBlock tbl_accuracy, CheckBox checkbox) : base(canvas, lbl_timer, tbl_hits, tbl_misses, tbl_accuracy, checkbox)
        {
            _rnd = new Random();
            this.canvas = canvas;
            this.lbl_timer = lbl_timer;
            this.tbl_hits = tbl_hits;
            this.tbl_misses = tbl_misses;
            this.tbl_accuracy = tbl_accuracy;
            this.checkbox = checkbox;
        }
        internal void Tick(object sender, EventArgs args)
        {

            if (_timerCounter <= 60)
            {
                canvas.IsEnabled = true;

                lbl_timer.Content = _timerCounter;
            }
            else
            {
                //timer.Stop();

                AccuracyCalc();
            }
            _timerCounter++;

        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="level"></param>
        public void SetTarget(Level level)
        {
            //if its null we assign the level to it
            if (_level == null) _level = level;

            //random target, max is the list length
            int nextTarget = _rnd.Next(0, level.Coordinates.Count);

            //choose a point from the list
            var point = level.Coordinates[nextTarget];

            Ellipse target = new Ellipse();
            target.Width = 30;
            target.Height = 30;
            target.Fill = Brushes.DarkBlue;
            target.Stroke = Brushes.Black;

            target.MouseLeftButtonDown += Target_MouseLeftButtonDownSetNextTarget;


            tbl_hits.Text = _hitCount.ToString();
            
            tbl_accuracy.Text = _accuracy.ToString();
            
            //add it to canvas
            Canvas.SetLeft(target, point.X - target.Width / 2);
            Canvas.SetTop(target, point.Y - target.Height / 2);
            canvas.Children.Add(target);
            //sets the current target
            _currentTarget = target;


        }
      
        public void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!_isHit)
            {
                _misses++;

                tbl_misses.Text = _misses.ToString();
            }
            _isHit = false;
        }

        internal void Target_MouseLeftButtonDownSetNextTarget(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;

            if (target != null && canvas.Children.Contains(target))
            {
                _isHit = true;
                target.MouseLeftButtonDown -= Target_MouseLeftButtonDownSetNextTarget;
                canvas.Children.Remove(target);
                

                _hitCount++;
            }
            
            if (_hitCount < _level.Coordinates.Count)
            {
                SetTarget(_level);

            }

        }
     

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            _Start = false;
            _customTargetIndex = 0;
            _targetCount = 0;
            _hitCount = 0;
            _misses = 0;
            _accuracy = 0;
            _timerCounter = 0;
            timer.Stop();
            timer.Tick -= Tick;
            canvas.Children.Clear();
            tbl_hits.Text = string.Empty;
            tbl_accuracy.Text = string.Empty;
            tbl_misses.Text = string.Empty;
            lbl_timer.Content = string.Empty;

            canvas.IsHitTestVisible = true;

        }
        private void sld_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SizeSlider = Math.Min(e.NewValue, 80);

        }

    }
}
