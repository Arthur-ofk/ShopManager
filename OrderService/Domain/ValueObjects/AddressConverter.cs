using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Domain.ValueObjects
{
    public class AddressConverter : ValueConverter<Address, string>
    {
        public AddressConverter()
            : base(
                address => $"{address.Street}, {address.City}, {address.Country}",
                str => ConvertStringToAddress(str))
        {
        }

        private static Address ConvertStringToAddress(string str)
        {
            var parts = str.Split(", ");
            return parts.Length == 3
                ? new Address(parts[0], parts[1], parts[2])
                : new Address("", "", "");
        }
    }
}
