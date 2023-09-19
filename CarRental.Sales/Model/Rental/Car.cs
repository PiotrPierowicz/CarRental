using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.Sales.Model.Rental;

public record Car : IValidatable, INotificationProducer<DomainError>
{
    public required int CarNumber { get; init; }
    public Notification<DomainError> Notification { get; } = new();
    public required decimal Price { get; init; }
    
    public bool Validate()
    {
        if(CarNumber >= 100000)
            Notification.AddError(new() {Message = "Car number has wrong format", Source = nameof(Car)});
        if(Price <= 0)
            Notification.AddError(new() {Message = "Price could not be 0 or below", Source = nameof(Car)});
        
        return !Notification.HasErrors;
    }
}