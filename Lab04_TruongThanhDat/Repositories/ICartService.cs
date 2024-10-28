using Lab04_TruongThanhDat.Models;

namespace Lab04_TruongThanhDat.Repositories
{
	public interface ICartService
	{
		List<CartItem> GetCartItems();
		void AddItem(int productId, int quantity);
		void RemoveItem(int productId);
		void ClearCart();
	}
}
