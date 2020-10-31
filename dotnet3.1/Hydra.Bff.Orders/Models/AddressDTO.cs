using System;

namespace Hydra.Bff.Orders.Models
{
    public class AddressDTO
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        public Guid CustomerID { get; set; }
    }
}