using Domain;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICustodiaRepository
    {
        Task<PosicaoConsolidada> GetByIdAsync(int id);
    }
}
