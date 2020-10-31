using System;

namespace Hydra.Bff.Orders.Models
{
    public class CatalogItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Qty { get; set; }
    }
}