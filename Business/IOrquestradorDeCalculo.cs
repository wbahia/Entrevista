using System.Threading.Tasks;

namespace Business
{
    public interface IOrquestradorDeCalculo
    {
        Task ComprarAsync(int clienteID, string ticker, int qdte);
    }
}