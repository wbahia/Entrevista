using Domain;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Business
{
    public class NotificacaoToro : INotificacaoObserver
    {
        public async Task NotificarAsync(PosicaoConsolidada posicaoConsolidada)
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
