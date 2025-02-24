using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace SemesterProjekt
{
    /// <summary>
    /// Interaktionslogik für PredefinedMode.xaml
    /// </summary>
    public partial class PredefinedMode : Window
    {

        public ObservableCollection<Level> Levels { get; set; }

        private Level _level;

        public PredefinedMode()
        {
            InitializeComponent();
            //Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            PredefinedClass predefinedClass = new PredefinedClass(canvas,lbl_timer,tbl_hits,tbl_misses,tbl_accuracy,checkbox);

            this.Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Crosshair.cur")).Stream);
            canvas.MouseLeftButtonDown += predefinedClass.Canvas_MouseLeftButtonDown;
            btn_reset.Click += predefinedClass.btn_Reset_Click;
            
            

        }
        

        private void btn_mainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json Datei (*.json)|*.json|Alle Dateien (*.*)|*.*";


            _level = null;


            if (openFileDialog.ShowDialog() == true)
            {

                _level = JsonLoader.ReadFromJsonFile<Level>(openFileDialog.FileName);
                
                string use = Directory.GetCurrentDirectory() + _level.BackgroundImage ;
                

                if (_level != null)
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(use));
                    imageBrush.Stretch = Stretch.UniformToFill;

                    canvas.Background = imageBrush;
                    

                }
            }

        }
    
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            PredefinedClass predefinedClass = new PredefinedClass(canvas, lbl_timer, tbl_hits, tbl_misses, tbl_accuracy, checkbox);
            predefinedClass.SetTarget(_level);

        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
