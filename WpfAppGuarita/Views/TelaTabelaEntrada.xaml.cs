using System;
using System.Collections.ObjectModel;
//using System.Data.SqlClient;
//using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

//using WpfAppGuarita;
//using WpfAppGuarita.Views;

namespace WpfAppGuarita.Views
{
    public class Ordem
    {
        public Int32 Numero { get; set; }
        public string Descricao { get; set; }
        public string DataServicoFmt { get; set; }
        public string DataVencimentoFmt { get; set; }
        public byte Status { get; set; }
        public string ValorFormatado { get; set; }
    }

    public partial class TelaTabelaEntrada : Page
    {
        //public ObservableCollection<Ordem> Ordens { get; set; }
        public ObservableCollection<Ordem> Ordens { get; } = new ObservableCollection<Ordem>();

        public Banco Sql { get; set; }

        public TelaTabelaEntrada()
        {
            InitializeComponent();

            //var model = new OrdensViewModel();
            //Ordens = model.Ordens;
            DataContext = this;

            // Rafael, comente esse código de baixo caso o C# não econtre o Sql Server no seu Perfil -Vitor
            Sql = new Banco();

            foreach (ContatoModel item in Sql.ViewBanco())
                Ordens.Add(new Ordem
                {
                    Numero = item.Id,
                    Descricao = item.Nome,
                    DataServicoFmt = item.Email,
                    DataVencimentoFmt = "28/05/2026",
                    Status = item.Esconder,
                    ValorFormatado = "R$ 180,00"
                });
        }

        // Trigger animation when the window is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a fade-in animation
                DoubleAnimation fadeIn = new DoubleAnimation
                {
                    From = 0,          // Start fully transparent
                    To = 1,            // End fully visible
                    Duration = TimeSpan.FromSeconds(1.5),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                // Start the animation on the TextBlock's Opacity property
                Atualizar.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Animation error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnAtualizar(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Navegar(new TelaTabelaEntrada());
        }

        private void OnVoltar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString(), e);
            MainWindow.Instance.Navegar(new TelaCapturaPlaca());
        }
    }
}
