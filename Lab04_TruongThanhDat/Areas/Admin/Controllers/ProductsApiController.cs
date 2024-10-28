using Lab04_TruongThanhDat.Models;
using Lab04_TruongThanhDat.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab04_TruongThanhDat.Areas.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsApiController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		public ProductsApiController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		//=============== GET PRODUCT
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			try
			{
				var products = await _productRepository.GetAllAsync();
				return Ok(products);
			}
			catch (Exception ex)
			{
				// Handle exception

				return StatusCode(500, "Internal server error");

			}
		}

		//===============  GET: api/ProductsApi/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return product;
		}

		//=============== PUT: api/ProductsApi/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, Product product)
		{
			if (id != product.Id)
			{
				return BadRequest();
			}
			try
			{
				await _productRepository.UpdateAsync(product);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
			}
			return Ok(new { message = "Cập nhật sản phẩm thành công." });
		}

		//=============== POST
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product)
		{
			_productRepository.AddAsync(product);
			return Ok(new { message = "Thêm phẩm thành công" }); // Trả về JSON thành công
		}


		//=============== DELETE: api/ProductsApi/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			_productRepository.DeleteAsync(id);
			return Ok(new { message = "Xoá sản phẩm thành công" }); // Trả về JSON thành công
		}
	}
}
