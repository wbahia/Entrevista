using System.Threading.Tasks;

namespace Business
{
    public interface IOrquestradorDeCalculo
    {
        Task ComprarAsync(int cliente, string ticker, int qdte);
    }
}