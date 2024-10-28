using Lab04_TruongThanhDat.Models;
using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab04_TruongThanhDat.Controllers
{
	public class HomeController : Controller
	{

		private readonly IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<IActionResult> Index()
		{
			var product = await _productRepository.GetAllAsync();
			return View(product);
		}
		public IActionResult About()
		{

			return View();
		}
		public async Task<IActionResult> Menu()
		{
			var product = await _productRepository.GetAllAsync();
			return View(product);
		}
		public async Task<IActionResult> SearchProduct(string searchString)
		{
			var products = await _productRepository.SearchAsync(searchString);
			if(products == null) 
			{ 
				return RedirectToAction("Index");
			}
			return View("SearchProduct", products);
		}

		public IActionResult Invoke()
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			int numberOfItems = cart.Items.Sum(item => item.Quantity);
			ViewData["NumberOfItems"] = numberOfItems;
			return View(cart);
		}
	}
}