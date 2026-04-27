using Academy.Services.Interfaces;
using Academy.Helpers;
using Academy.ViewModels.Basket;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Academy.Services.Interfaces;

namespace Academy.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public bool AddToBasket(int courseId, string userId = null)
        {
            var basket = Session.GetObjectFromJson<List<BasketItemDto>>("Basket") ?? new List<BasketItemDto>();

            if (!basket.Any(b => b.CourseId == courseId))
            {
                basket.Add(new BasketItemDto { CourseId = courseId });
                Session.SetObjectAsJson("Basket", basket);
                return true;
            }
            return false;
        }

        public void RemoveFromBasket(int courseId)
        {
            var basket = Session.GetObjectFromJson<List<BasketItemDto>>("Basket");

            if (basket != null)
            {
                var itemToRemove = basket.FirstOrDefault(b => b.CourseId == courseId);
                if (itemToRemove != null)
                {
                    basket.Remove(itemToRemove);
                    Session.SetObjectAsJson("Basket", basket);
                }
            }
        }

        public List<int> GetBasketCourseIds()
        {
            var basket = Session.GetObjectFromJson<List<BasketItemDto>>("Basket");
            if (basket == null) return new List<int>();

            return basket.Select(b => b.CourseId).ToList();
        }
    }
}