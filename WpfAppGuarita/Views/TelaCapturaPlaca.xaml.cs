using MjpegProcessor;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfAppGuarita.Views
{
    public partial class TelaCapturaPlaca : Page
    {
        private MjpegDecoder _mjpegDecoder;
        private DispatcherTimer _timerPlaca;
        private readonly HttpClient _httpClient = new HttpClient();
        private System.Windows.Media.Brush CorLimpar { get; set; }

        public TelaCapturaPlaca()
        {
            InitializeComponent();

            CorLimpar = BtnLimpar.Background;

            // 1. Inicializa o Decoder de Vídeo (MjpegProcessor)
            _mjpegDecoder = new MjpegDecoder();
            _mjpegDecoder.FrameReady += MjpegDecoder_FrameReady;

            // 2. Inicia o Timer para buscar o texto da placa lida pelo Python
            _timerPlaca = new DispatcherTimer();
            _timerPlaca.Interval = TimeSpan.FromMilliseconds(1000); // 1 segundo
            _timerPlaca.Tick += TimerPlaca_Tick;
            _timerPlaca.Start();

            // 3. Conecta no Stream do Python
            ConectarCamera();
        }

        private void ConectarCamera()
        {
            try
            {
                // Inicia a recepção dos frames do servidor Flask
                _mjpegDecoder.ParseStream(new Uri("http://localhost:5000/video_feed"));

                // Se houver o txtStatus (invisível), marcamos como online
                if (txtStatus != null) txtStatus.Text = "Online";
            }
            catch (Exception ex)
            {
                txtPlacaDetectada.Text = "ERRO DE CONEXÃO";
                MessageBox.Show("Certifique-se que o script Python está rodando!\n" + ex.Message);
            }
        }

        // Atualiza a imagem na tela sempre que um novo frame chega do Python
        private void MjpegDecoder_FrameReady(object sender, FrameReadyEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    using (var ms = new MemoryStream(e.FrameBuffer))
                    {
                        var bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.StreamSource = ms;
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        bmp.Freeze(); // Necessário para evitar erro de thread na UI

                        WebcamImage.Source = bmp;
                    }
                }
                catch { /* Ignora frames corrompidos */ }
            }), DispatcherPriority.Render);
        }

        // Busca o resultado do OCR que o Python guardou
        private async void TimerPlaca_Tick(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.GetStringAsync("http://localhost:5000/get_placa");
                var resultado = JsonConvert.DeserializeObject<dynamic>(response);
                string placa = resultado.placa;

                Dispatcher.Invoke(() => {
                    // Se o Python resetou, o C# também deve mostrar que está procurando
                    if (placa == "Procurando...")
                    {
                        txtPlacaDetectada.Text = "AGUARDANDO...";
                        txtPlacaDetectada.Foreground = System.Windows.Media.Brushes.Gray;
                        BtnLimpar.Background = CorLimpar;
                    }
                    else
                    {
                        txtPlacaDetectada.Text = placa;
                        txtPlacaDetectada.Foreground = System.Windows.Media.Brushes.Green;
                        BtnLimpar.Background = System.Windows.Media.Brushes.Green;
                    }
                });
            }
            catch { }
        }


        
        // Botão para limpar a placa atual e permitir nova leitura
        private async void OnLimpar(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Avisa o Python para resetar a detecção
                await _httpClient.GetAsync("http://localhost:5000/reset");

                // 2. Reseta o visual no WPF
                txtPlacaDetectada.Text = "AGUARDANDO PLACA...";
                txtPlacaDetectada.Foreground = System.Windows.Media.Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao resetar servidor: " + ex.Message);
            }
        }

        private async void BtnSalvarFoto(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Avisa o Python para salvar a ultima placa que passa os requerimentos
                await _httpClient.GetAsync("http://localhost:5000/salvar");

                // 2. Reseta o visual no WPF
                txtPlacaDetectada.Text = "Imagem Salva";
                txtPlacaDetectada.Foreground = System.Windows.Media.Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar imagem: " + ex.Message);
            }
        }

        // Método para fechar a conexão ao sair da página (evita travar a webcam)
        public void Finalizar()
        {
            _mjpegDecoder?.StopStream();
            _timerPlaca?.Stop();
        }

        private void BtnRegistros(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString(), e);
            MainWindow.Instance.Navegar(new TelaTabelaEntrada());
        }
    }
}