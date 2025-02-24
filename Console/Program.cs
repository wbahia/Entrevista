using Business;
using Newtonsoft.Json;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICustodiaRepository repository = new CustodiaRepository();
            List<INotificacaoObserver> observers = new List<INotificacaoObserver>();
            IOrquestradorDeCalculo bo = new OrquestradorDeCalculo(repository, observers);

            var posicaoCliente = await repository.GetByIdAsync(1);

            var json = JsonConvert.SerializeObject(posicaoCliente);
            System.Console.WriteLine($"POSIÇÃO INICIAL");
            System.Console.WriteLine($"{json}");

            await bo.ComprarAsync(1, "PETR3", 2);

            System.Console.WriteLine("End!");
            System.Console.ReadLine();
        }
    }
}
