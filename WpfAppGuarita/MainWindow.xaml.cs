using System.Windows;
using System.Windows.Controls;

namespace WpfAppGuarita
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Página inicial
            Navegar(new Views.TelaInicial());
        }

        public void Navegar(Page pagina)
        {
            MainFrame.Navigate(pagina);
        }
    }
}