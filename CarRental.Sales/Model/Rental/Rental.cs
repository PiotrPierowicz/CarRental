using CarRental.BuildingBlocks.DDD;
using CarRental.HR.Model;
using CarRental.Sales.Events;

namespace CarRental.Sales.Model;

public class Rental: AggregateRoot
{
    public required string RentalIdentifier { get; init; }
    public required Address OfficeAddress { get; init; }
    public required HashSet<RentalEmployee> RentalEmployees { get; init; }
    public required HashSet<Rent> Rents { get; init; }
    protected bool Equals(Rental other)
    {
        return RentalIdentifier == other.RentalIdentifier && OfficeAddress.Equals(other.OfficeAddress);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Rental)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RentalIdentifier, OfficeAddress);
    }

    public static bool operator ==(Rental? left, Rental? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Rental? left, Rental? right)
    {
        return !Equals(left, right);
    }

    public required HashSet<Car> CarsAvailableOnSide { get; init; }

    public string RentingACar(RentalEmployee employee, int carNumber, Client client, DateTime start, DateTime end)
    {
        if (!employee.Validate()) Notification.AddErrors(employee.Notification.Errors);;
        
        if (employee.OnLeave)
            Notification.AddError(new() { Message = "Employee is on leave, he cannot rent a car", Source = nameof(Rental)});
        
        if (!client.Validate()) Notification.AddErrors(client.Notification.Errors);;
        
        var car = CarsAvailableOnSide.SingleOrDefault(x => x.CarNumber.Equals(carNumber));
        //no "remote" rents yet
        if (car is null)
            Notification.AddError(new() { Message = "No such available car in this rental", Source = nameof(Rental)});
        
        if (Notification.HasErrors) throw new DomainException(Notification);
        
        CarsAvailableOnSide.Remove(car);

        string rentNumber = GenerateRentNumber();

        Rents.Add(new ()
        {
            Start = start,
            End = end,
            Client = client,
            RentNumber = rentNumber,
            Car = car
        });
        
        return rentNumber;
    }

    public Rent ConfirmRent(string rentNumber)
    {
        var rent = Rents.Single(x => x.RentNumber.Equals(rentNumber));
        //calculate price - in a future, we can implement promotions here.
        decimal price = (decimal)(rent.End - rent.Start).TotalDays * rent.Car.Price;
        rent.Confirm(price);
        
        Events.Add(new CarRentOnSide
        {
            RentNumber = rentNumber,
            FirstName = rent.Client.FirstName,
            LastName = rent.Client.LastName,
            CorrespondencyAddress = rent.Client.CorrespondencyAddress,
            IdNumber = rent.Client.IdNumber,
            IsCompany = rent.Client.IsCompany,
            RentalIdentifier = this.RentalIdentifier,
            Start = rent.Start,
            End = rent.End,
            CarNumber = rent.Car.CarNumber
        });

        return rent;
    }
    
    public override bool Validate()
    {
        if(string.IsNullOrEmpty(RentalIdentifier))
            Notification.AddError(new() { Message = "Rental does not have identifier", Source = nameof(Rental)});
        
        if(OfficeAddress is null)
            Notification.AddError(new() {Message = "Office address could not be null", Source = nameof(Rental)});
        
        if(!OfficeAddress.Validate())
            Notification.AddErrors(OfficeAddress.Notification.Errors);
            
        return !Notification.HasErrors;
    }

    private string GenerateRentNumber()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}