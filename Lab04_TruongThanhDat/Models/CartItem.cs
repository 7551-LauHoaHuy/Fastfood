namespace Lab04_TruongThanhDat.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        // Thông tin số lượng
        public int Quantity { get; set; }
    }
}
