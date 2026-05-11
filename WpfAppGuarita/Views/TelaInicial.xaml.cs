using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppGuarita.Views
{
    public partial class TelaInicial : Page
    {
        public TelaInicial()
        {
            InitializeComponent();
        }

        private void OnIniciar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString(), e);
            MainWindow.Instance.Navegar(new TelaCapturaPlaca());
        }

        private void OnRegistros(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString(), e);
            MainWindow.Instance.Navegar(new TelaTabelaEntrada());
        }
    }
}
