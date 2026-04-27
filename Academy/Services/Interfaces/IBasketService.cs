using System.Collections.Generic;

namespace Academy.Services.Interfaces
{
    public interface IBasketService
    {
        bool AddToBasket(int courseId, string userId = null);
        void RemoveFromBasket(int courseId);
        List<int> GetBasketCourseIds();
    }
}