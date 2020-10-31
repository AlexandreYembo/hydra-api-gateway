namespace Hydra.Bff.Orders.Models
{
    public class VoucherDTO
    {
        public decimal? Discount { get; set; }
        public string Code { get; set; }
        public int DiscountType { get; set; }
    }
}