using Domain;
using Newtonsoft.Json;
using Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class OrquestradorDeCalculo : IOrquestradorDeCalculo
    {
        private readonly ICustodiaRepository _repository;

        public OrquestradorDeCalculo(ICustodiaRepository repository)
        {
            _repository = repository;
        }

        public async Task ComprarAsync(int cliente, string ticker, int qdte)
        {
            try
            {
                var posicaoConsolidada = await _repository.GetByIdAsync(cliente);

                CalculoFinanceiro(posicaoConsolidada, ticker, qdte);
                CalculoCustodia(posicaoConsolidada, ticker, qdte);

                await NotificarCliente(posicaoConsolidada);
                await NotificarToro(posicaoConsolidada);
            }
            catch (Exception ex)
            {
                Monitoramento(ex);
            }
        }

        private void CalculoFinanceiro(PosicaoConsolidada posicaoConsolidada, string ticker, int qdte)
        {
            var ativo = posicaoConsolidada.Custodia.FirstOrDefault(o => o.Ticker == ticker);
            var vlCompra = ativo.CotacaoHoje * qdte;

            posicaoConsolidada.Financeiro -= vlCompra;
        }

        private void CalculoCustodia(PosicaoConsolidada posicaoConsolidada, string ticker, int qdte)
        {
            var ativo = posicaoConsolidada.Custodia.FirstOrDefault(o => o.Ticker == ticker);
            ativo.Quantidade += qdte;
        }

        private void Monitoramento(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private async Task NotificarCliente(PosicaoConsolidada posicaoConsolidada)
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(posicaoConsolidada);
                Console.WriteLine();
                Console.WriteLine("Notificação CLIENTE");
                Console.WriteLine($"{json}");
            });
        }

        private async Task NotificarToro(PosicaoConsolidada posicaoConsolidada)
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(posicaoConsolidada);
                Console.WriteLine();
                Console.WriteLine("Notificação TORO");
                Console.WriteLine($"{json}");
            });
        }
    }
}
