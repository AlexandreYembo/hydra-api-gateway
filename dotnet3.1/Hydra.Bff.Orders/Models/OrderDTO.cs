using System.Collections.Generic;

namespace Hydra.Bff.Orders.Models
{
    public class OrderDTO
    {
    public decimal TotalPrice { get; set; }
    public List<BasketItemDTO> Items { get; set; }
    public AddressDTO Address { get; set; }

    public PaymentDTO Payment { get; set; }
    public VoucherDTO Voucher { get; set; }
    public bool HasVoucher { get; set; }
    }
}