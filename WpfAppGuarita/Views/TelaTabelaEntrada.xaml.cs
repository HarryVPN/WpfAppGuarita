using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;

using WpfAppGuarita;
using WpfAppGuarita.Views;


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

    //public class OrdensViewModel
    //{
    //    public ObservableCollection<Ordem> Ordens { get; } = new ObservableCollection<Ordem>();

    //    public OrdensViewModel()
    //    {
    //        //Ordens.Add(new Ordem
    //        //{
    //        //    Numero = "ABC-1234",
    //        //    Descricao = "Toyota Corolla",
    //        //    DataServicoFmt = "10/05/2026",
    //        //    DataVencimentoFmt = "20/05/2026",
    //        //    Status = "Aberto",
    //        //    ValorFormatado = "R$ 350,00"
    //        //});

    //        //Ordens.Add(new Ordem
    //        //{
    //        //    Numero = "XYZ-9876",
    //        //    Descricao = "Honda Civic",
    //        //    DataServicoFmt = "11/05/2026",
    //        //    DataVencimentoFmt = "25/05/2026",
    //        //    Status = "Pago",
    //        //    ValorFormatado = "R$ 420,00"
    //        //});
    //    }
    //}

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
            DataContext = Ordens;

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
