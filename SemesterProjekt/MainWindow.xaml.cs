using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemesterProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            


        }

        private void Standard_Click(object sender, RoutedEventArgs e)
        {
            Standard standardMode = new Standard();
            standardMode.Show();
            this.Close();


        }
        private void Predefined_Click(object sender, RoutedEventArgs e)
        {
       
        }

        private void Creator_Click(object sender, RoutedEventArgs e)
        {
            CreatorMode creatorMode = new CreatorMode();
            creatorMode.Show();
            this.Close();
        }
    }
}