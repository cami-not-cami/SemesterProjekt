using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace SemesterProjekt
{
    /// <summary>
    /// Interaktionslogik für CreatorMode.xaml
    /// </summary>
    public partial class CreatorMode : Window
    {
        
        public CreatorMode(Canvas canvas, Label lbl_timer, TextBlock tbl_hits, TextBlock tbl_misses, TextBlock tbl_accuracy, CheckBox checkbox)
        {
            this.canvas = canvas;
            this.lbl_timer = lbl_timer;
            this.tbl_hits = tbl_hits;
            this.tbl_misses = tbl_misses;
            this.tbl_accuracy = tbl_accuracy;
            this.checkbox = checkbox;
        }
        public CreatorMode()
        {
            bool iscustom=true;
            InitializeComponent();
            Target target = new Target(canvas,lbl_timer,tbl_hits,tbl_misses,tbl_accuracy,checkbox);
            target.StartGame(iscustom);

            this.Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Crosshair.cur")).Stream);
            canvas.MouseLeftButtonDown += target.Canvas_MouseLeftButtonDown;
            btn_start.Click += target.btn_start_Click;
            canvas.MouseRightButtonDown += target.Canvas_MouseRightButtonDown;



        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
           
            if(openFileDialog.ShowDialog()==true)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource= new BitmapImage(new Uri(openFileDialog.FileName,UriKind.Absolute));
                imageBrush.Stretch = Stretch.UniformToFill;

                canvas.Background = imageBrush;
            }

          

        }

     
    }
}
