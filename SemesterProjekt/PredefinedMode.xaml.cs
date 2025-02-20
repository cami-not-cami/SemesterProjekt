using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für PredefinedMode.xaml
    /// </summary>
    public partial class PredefinedMode : Window
    {

        public ObservableCollection<Level> Levels { get; set; }
        public PredefinedMode()
        {
            InitializeComponent();
            
        }

        private void btn_mainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json Datei (*.json)|*.json|Alle Dateien (*.*)|*.*";


            Level level = null;


            if (openFileDialog.ShowDialog() == true)
            {

                level = JsonLoader.ReadFromJsonFile<Level>(openFileDialog.FileName);
                if(level != null)
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(level.BackgroundImage));
                    imageBrush.Stretch = Stretch.UniformToFill;

                    canvas.Background = imageBrush;
                
                    
                }

            }
        }
        private void sld_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
