using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

//using System.Data.SqlClient;
//using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using WpfAppGuarita;
//using WpfAppGuarita.Views;

namespace WpfAppGuarita.Views
{
    public partial class TelaRegistros : Page
    {
        public ObservableCollection<RegistroModel> Ordens { get; } = new ObservableCollection<RegistroModel>();
        //public Banco Sql { get; set; }
        public HttpCarlosAPI Api { get; } = new HttpCarlosAPI();

        public async Task ReultadoDaApiGeraLista()
        {
            List<RegistroModel> carros = await Api.ListagemCarros();

            foreach (var registro in carros!)
            {
                Ordens.Add(new RegistroModel
                {
                    dataAgendamento = registro.dataAgendamento,
                    cliente = registro.cliente,
                    placa = registro.placa,
                    tecnico = registro.tecnico,
                    agendadoPor = registro.agendadoPor
                });
            }
            Esperando.Text = "";
        }

        public TelaRegistros()
        {
            InitializeComponent();
            DataContext = this;

            HttpCarlosAPI utils = new HttpCarlosAPI();

            ReultadoDaApiGeraLista();
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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Animation error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnAtualizar(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Navegar(new TelaRegistros());
        }

        private void OnVoltar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString(), e);
            MainWindow.Instance.Navegar(new TelaCapturaPlaca());
        }
    }
}
