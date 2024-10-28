using Lab04_TruongThanhDat.Models;

namespace Lab04_TruongThanhDat.Repositories
{
	public class EFCartService : ICartService
	{
		private List<CartItem> cartItems = new List<CartItem>();

		public List<CartItem> GetCartItems()
		{
			return cartItems;
		}

		public void AddItem(int productId, int quantity)
		{
			var existingItem = cartItems.Find(item => item.Id == productId);
			if (existingItem != null)
			{
				existingItem.Quantity += quantity;
			}
			else
			{
				cartItems.Add(new CartItem { Id = productId, Quantity = quantity });
			}
		}

		public void RemoveItem(int productId)
		{
			cartItems.RemoveAll(item => item.Id == productId);
		}

		public void ClearCart()
		{
			cartItems.Clear();
		}
	}
}
