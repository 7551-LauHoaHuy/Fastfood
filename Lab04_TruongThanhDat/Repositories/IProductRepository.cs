using Lab04_TruongThanhDat.Models;

namespace Lab04_TruongThanhDat.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

		Task<List<Product>> SearchAsync(string searchString);
	}
}
