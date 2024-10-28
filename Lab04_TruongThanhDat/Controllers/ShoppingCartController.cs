using CodeMegaVNPay.Services;
using Lab04_TruongThanhDat.Models;
using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sek.Module.Payment.VnPay;



namespace Lab04_TruongThanhDat.Controllers
{

	public class ShoppingCartController : Controller
	{
		private readonly IVnPayService _vnPayService;
		private readonly IProductRepository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ShoppingCartController(IProductRepository productRepository,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager, IVnPayService vnPayService)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;
			_vnPayService = vnPayService;
		}


		//================ CHECK OUT
		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			return View(new Order());
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Checkout(Order order)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart == null || !cart.Items.Any())
			{
				// Xử lý giỏ hàng trống...
				return RedirectToAction("Index");
			}
			var user = await _userManager.GetUserAsync(User);
			order.UserId = user.Id;
			order.OrderDate = DateTime.UtcNow;
			order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
			order.OrderDetails = cart.Items.Select(i => new OrderDetail
			{
				ProductId = i.Id,
				Quantity = i.Quantity,
				Price = i.Price
			}).ToList();
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			HttpContext.Session.Remove("Cart");
			return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
		}


		//================ INDEX
		public IActionResult Index()
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			int numberOfItems = cart.Items.Sum(item => item.Quantity);
			ViewData["NumberOfItems"] = numberOfItems;
			return View(cart);
		}

		//================ ADD TO CART
		[Authorize]
		public async Task<IActionResult> AddToCart(int productId, int quantity)
		{
			if (!User.Identity.IsAuthenticated)
			{
				// User is not logged in, redirect to login page
				return RedirectToAction("Login"); // Replace with your login action
			}
			// Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
			var product = await GetProductFromDatabase(productId);
			var cartItem = new CartItem
			{
				Id = productId,
				Name = product.Name,
				Image = product.ImageUrl,
				Price = product.Price,
				Quantity = quantity
			};
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);
			HttpContext.Session.SetObjectAsJson("Cart", cart);
			return RedirectToAction("Index");
		}//================ GET PRODUCT FROM DATABASE
		private async Task<Product> GetProductFromDatabase(int productId)
		{
			// Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
			var product = await _productRepository.GetByIdAsync(productId);
			return product;
		}

		//================ REMOVE
		public IActionResult RemoveFromCart(int productId)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart is not null)
			{
				cart.RemoveItem(productId);
			}
			// Lưu lại giỏ hàng vào Session sau khi đã xóa mục
			HttpContext.Session.SetObjectAsJson("Cart", cart);
			return RedirectToAction("Index");
		}
		[HttpPost]
		public IActionResult CreatePaymentUrl(PaymentInformationModel model)
		{
			var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

			return Redirect(url);
		}

		public IActionResult PaymentCallback()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);

			return Json(response);
		}


	}
}