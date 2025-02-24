using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class OrquestradorDeCalculo : IOrquestradorDeCalculo
    {
        private readonly ICustodiaRepository _repository;
        private readonly List<INotificacaoObserver> _observers;

        public OrquestradorDeCalculo(ICustodiaRepository repository, IEnumerable<INotificacaoObserver> observers)
        {
            _repository = repository;
            _observers = new List<INotificacaoObserver>();

            // Adicionando os observadores
            AdicionarObserver(new NotificacaoCliente());
            AdicionarObserver(new NotificacaoToro());
        }

        public void AdicionarObserver(INotificacaoObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoverObserver(INotificacaoObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task ComprarAsync(int clienteID, string ticker, int qdte)
        {
            try
            {
                var posicaoConsolidada = await _repository.GetByIdAsync(clienteID);

                CalculoFinanceiro(posicaoConsolidada, ticker, qdte);
                CalculoCustodia(posicaoConsolidada, ticker, qdte);

                await NotificarObservers(posicaoConsolidada);
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

        private async Task NotificarObservers(PosicaoConsolidada posicaoConsolidada)
        {
            foreach (var observer in _observers)
            {
                try
                {
                    await observer.NotificarAsync(posicaoConsolidada);
                }
                catch (Exception ex)
                {
                    // Log exception or handle it accordingly
                    Console.WriteLine($"Erro ao notificar observador: {ex.Message}");
                }
            }
        }
    }
}
