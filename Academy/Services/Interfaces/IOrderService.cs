using System.Threading.Tasks;

namespace Academy.Services.Interfaces
{
    public interface IOrderService
    {
        Task CheckoutAsync(string userId, List<int> courseIds);
    }
}