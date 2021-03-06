using System.Collections.Generic;

namespace Hydra.Bff.Orders.Models
{
    public class BasketDTO
    {
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public VoucherDTO Voucher { get; set; }
        public List<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();
        public bool HasVoucher { get; set; }
    }
}