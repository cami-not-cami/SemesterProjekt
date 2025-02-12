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
using System.Windows.Media.Animation;
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
     
        public Standard(Canvas canvas, Label lbl_timer, TextBlock tbl_hits, TextBlock tbl_misses, TextBlock tbl_accuracy, CheckBox checkbox)
        {
            this.canvas = canvas;
            this.lbl_timer = lbl_timer;
            this.tbl_hits = tbl_hits;
            this.tbl_misses = tbl_misses;
            this.tbl_accuracy = tbl_accuracy;
            this.checkbox = checkbox;
            
        }
        public Standard()
        {
            InitializeComponent();

            Target myTarget = new Target(canvas, lbl_timer, tbl_hits, tbl_misses, tbl_accuracy, checkbox);
            myTarget.StartSpawningTargets();

            //set file as resource first 
            this.Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Crosshair.cur")).Stream);
            canvas.MouseLeftButtonDown += myTarget.Canvas_MouseLeftButtonDown;
            btn_reset.Click += myTarget.btn_Reset_Click;
            btn_start.Click += myTarget.btn_start_Click;
            sld_slider.ValueChanged += myTarget.sld_slider_ValueChanged;
            
            


        }
        private void sld_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

     
    }
}
