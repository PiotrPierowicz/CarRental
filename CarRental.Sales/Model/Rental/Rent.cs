using CarRental.BuildingBlocks.DDD;

namespace CarRental.Sales.Model;

public class Rent: Entity
{
    public required Client Client { get; init; }
    public required Car Car { get; init; }
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public required string RentNumber { get; init; }
    public bool Confirmed { get; private set; }
    public decimal FinalPrice { get; private set; }

    public void Confirm(decimal finalPrice)
    {
        Confirmed = true;
        FinalPrice = finalPrice;
    }
    protected bool Equals(Rent other)
    {
        return Client.Equals(other.Client) && Car.Equals(other.Car) && Start.Equals(other.Start) && End.Equals(other.End) && RentNumber == other.RentNumber;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Rent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Client, Car, Start, End, RentNumber);
    }

    public static bool operator ==(Rent? left, Rent? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Rent? left, Rent? right)
    {
        return !Equals(left, right);
    }

    public override bool Validate()
    {
        if(Start >= End)
            Notification.AddError(new() {Message = "Rent dates are incorrect", Source = nameof(Rent)});

        if(!Client.Validate())
            Notification.AddErrors(Client.Notification.Errors);
        
        if(!Car.Validate())
            Notification.AddErrors(Car.Notification.Errors);
        
        if(string.IsNullOrEmpty(RentNumber))
            Notification.AddError(new() {Message = "RentNumber can't be a null", Source = nameof(Rent)});
        
        return !Notification.HasErrors;
    }
}