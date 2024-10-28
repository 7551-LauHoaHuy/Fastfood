using Lab04_TruongThanhDat.Areas.Admin.Models;
using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_TruongThanhDat.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class AdminHomeController : Controller
    {

        private readonly IProductRepository _productRepository;

        public AdminHomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _productRepository.GetAllAsync();
            return View(product);
        }
    }
}
