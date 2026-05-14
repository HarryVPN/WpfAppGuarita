using System;
using System.Collections.Generic;
using System.Text;
using WpfAppGuarita.Dtos;
using WpfAppGuarita.Models;

namespace WpfAppGuarita.IntegracaoAPI.Interface
{
    public interface IApiControleVeiculos
    {
        Task<List<RegistroModel>?> ListagemCarros();
        Task<bool> RegistrarEntrada(CriarNovoRegistro novoRegistro);
        Task<bool> PegarPlacaCapturada();
    }
}
