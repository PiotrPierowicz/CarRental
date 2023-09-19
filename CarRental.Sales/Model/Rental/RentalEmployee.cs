using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.Sales.Model;

public class RentalEmployee: Entity
{
    protected bool Equals(RentalEmployee other)
    {
        return FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RentalEmployee)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    public static bool operator ==(RentalEmployee? left, RentalEmployee? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RentalEmployee? left, RentalEmployee? right)
    {
        return !Equals(left, right);
    }

    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required bool OnLeave { get; init; }
    
    public override bool Validate()
    {
        if(
            string.IsNullOrEmpty(FirstName)
            || string.IsNullOrEmpty(LastName))
            Notification.AddError(new () {Message = "FirstName and LastName must be provided.", Source = nameof(RentalEmployee)});

        return !Notification.HasErrors;
    }
}