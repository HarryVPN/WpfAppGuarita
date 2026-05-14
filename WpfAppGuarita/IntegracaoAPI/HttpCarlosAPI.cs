using System.Net.Http;
using System.Text;
using System.Text.Json;
using WpfAppGuarita.Dtos;
using WpfAppGuarita.IntegracaoAPI.Interface;
using WpfAppGuarita.Models;

public class HttpCarlosAPI : IApiControleVeiculos
{
    private readonly HttpClient _client = new();
    private const string BaseUrl = "http://10.1.93.36:5197/api/ControleVeiculoMovimento";

    public async Task<List<RegistroModel>?> ListagemCarros()
    {
        HttpResponseMessage response = await _client.GetAsync($"{BaseUrl}/Lista");

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseBody);

        if (responseBody != null)
            return JsonSerializer.Deserialize<List<RegistroModel>>(responseBody);
        return null;
    }

    private class JsonPlaca
    {
        public string placa { get; set; }
    }

    public async Task testepost(string placa)
    {
        string jsonString = JsonSerializer.Serialize(new JsonPlaca {placa=placa});
        Console.WriteLine(jsonString);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{BaseUrl}/registrarEntrada", content);

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        response.EnsureSuccessStatusCode();
        Console.WriteLine(response);
        //var postData = new
        //{
        //    name = "John Doe",
        //    email = "john@example.com"
        //};

        //// Serialize object to JSON
        //string json = JsonSerializer.Serialize(postData);

        //// Create HttpClient instance (reuse in real apps)
        //using HttpClient client = new HttpClient();

        //// Set request content type to JSON
        //using var content = new StringContent(json, Encoding.UTF8, "application/json");

        //try
        //{
        //    // Send POST request
        //    HttpResponseMessage response = await client.PostAsync("http://10.1.93.36:5197/api/ControleVeiculoMovimento/registrarEntrada", content);

        //    // Ensure success status code
        //    response.EnsureSuccessStatusCode();

        //    // Read response body
        //    string responseBody = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine("Response:");
        //    Console.WriteLine(responseBody);
        //}
        //catch (HttpRequestException e)
        //{
        //    Console.WriteLine($"Request error: {e.Message}");
        //}
        //catch (TaskCanceledException)
        //{
        //    Console.WriteLine("Request timed out.");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Unexpected error: {ex.Message}");
        //}
    }

    public async Task<bool> RegistrarEntrada(CriarNovoRegistro novoRegistro)
    {
        string json = JsonSerializer.Serialize(novoRegistro);

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync($"{BaseUrl}/registrarEntrada", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PegarPlacaCapturada()
    {
        string urlPython = "http://localhost:5000/salvar";

        var responsePython = await _client.GetAsync(urlPython);

        if (!responsePython.IsSuccessStatusCode) return false;

        var jsonPython = await responsePython.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(jsonPython);

        string? placaIndentificada = doc.RootElement.GetProperty("placa").GetString();

        if (placaIndentificada == null) return false;

        var novoRegistro = new CriarNovoRegistro
        {
            placa = placaIndentificada
        };

        return await RegistrarEntrada(novoRegistro);
    }
}