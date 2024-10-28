using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab04_TruongThanhDat.Models
{
	public class CartSummaryViewComponent : ViewComponent
	{
		
		private readonly ICartService _cartService;
		/*public CartSummaryViewComponent() { }*/
		public CartSummaryViewComponent(ICartService cartService)
		{
			_cartService = cartService;
		}

		public IViewComponentResult Invoke()
		{
			// Ensure _cartService is not null before using it
			if (_cartService != null)
			{
				List<CartItem> cart = _cartService.GetCartItems();

				CartSummaryViewModel viewModel = new CartSummaryViewModel();
				viewModel.NumOfItems = cart.Count;

				return View(viewModel);
			}

			// Handle the case where _cartService is null
			else
			{
				// Log error or display appropriate message
				return View("Error", new ErrorViewModel { ErrorMessage = "CartService is null" });
			}
		}
			/*public IViewComponentResult Invoke()
			{
				List<CartItem> cart = GetCartItems();
				CartSummaryViewModel viewModel = new CartSummaryViewModel();
				viewModel.NumOfItems = cart.Count;
				return View(viewModel);
			}
			public List<CartItem> GetCartItems()
			{
				var sessionCart = HttpContext.Session.GetString("cart");
				if (sessionCart != null)
				{
					return JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
				}
				return new List<CartItem>();
			}*/
		}
	}
