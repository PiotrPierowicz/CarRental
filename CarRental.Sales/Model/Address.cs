using CarRental.BuildingBlocks;
using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.Sales.Model;

public class Address: Entity
{
    public required string Street {get; init;}
    public required string HouseNumber {get; init;}
    public required string ZipCode {get; init;}
    public required string City {get; init;}

    public override bool Validate()
    {
        if(
            string.IsNullOrEmpty(Street) ||
            string.IsNullOrEmpty(HouseNumber) ||
            string.IsNullOrEmpty(ZipCode) ||
            string.IsNullOrEmpty(City) 
            )
            return false;
        return true;
    }

    public virtual bool Equals(Address? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Street,
                   other.Street,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(HouseNumber,
                   other.HouseNumber,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(ZipCode,
                   other.ZipCode,
                   StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(City,
                   other.City,
                   StringComparison.CurrentCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Street, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(HouseNumber, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(ZipCode, StringComparer.CurrentCultureIgnoreCase);
        hashCode.Add(City, StringComparer.CurrentCultureIgnoreCase);
        return hashCode.ToHashCode();
    }
}
