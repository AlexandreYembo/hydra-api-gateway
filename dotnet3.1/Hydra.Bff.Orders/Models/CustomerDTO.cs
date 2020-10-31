namespace Hydra.Bff.Orders.Models
{
    public class CustomerDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string IdentityNumber { get; set; }
        public bool IsRemoved { get; set; }
        public AddressDTO Address { get; set; }
    }
}