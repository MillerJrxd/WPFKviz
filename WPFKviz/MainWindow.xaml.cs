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

namespace WPFKviz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string connectionString = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow();
            quizWindow.Show();
            this.Close();
        }

        private void ImportDatabase_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Adatbázis importálása funkció hamarosan!");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Beállítások funkció hamarosan!");
        }
    }
}
