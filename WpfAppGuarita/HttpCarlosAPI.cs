using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class RegistroModel
{
    public DateTime dataAgendamento { get; set; }
    public string cliente { get; set; }
    public string placa { get; set; }
    public string tecnico { get; set; }
    public string agendadoPor { get; set; }
}

public class HttpCarlosAPI
{

    public async Task<List<RegistroModel>> ListagemCarros()
    {
        HttpClient client = new HttpClient();

        string url = "http://10.1.93.36:5197/api/ControleVeiculoMovimento/Lista";

        HttpResponseMessage response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseBody);

        return JsonSerializer.Deserialize<List<RegistroModel>>(responseBody);
    }
}