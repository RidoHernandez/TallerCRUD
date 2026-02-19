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

namespace CRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarFecha();
        }

        public void CargarFecha()
        {
            lblFecha.Content = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void Refacciones_Click(object sender, RoutedEventArgs e)
        {
            Refacciones refacciones = new Refacciones(this);
            refacciones.Show();
            this.Hide();
        }

        private void Servicios_Click(object sender, RoutedEventArgs e)
        {
            Servicios servicios = new Servicios(this);
            servicios.Show();
            this.Hide();
        }
    }
}