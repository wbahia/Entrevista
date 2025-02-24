using Domain;
using System.Threading.Tasks;

namespace Business
{
    public interface INotificacaoObserver
    {
        Task NotificarAsync(PosicaoConsolidada posicaoConsolidada);
    }
}