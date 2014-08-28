
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart cart { get; set; }
        public string returnUrl { get; set; }
    }
}
