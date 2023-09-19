using CarRental.BuildingBlocks.DDD;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;
using CarRental.HR.Model;

namespace CarRental.Sales.Model;

public record Client: IValidatable, INotificationProducer<DomainError>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Address CorrespondencyAddress { get; init; }
    public required string IdNumber { get; init; }
    public string? CompanyName { get; init; }
    public Address? CompanyAddress { get; init; }
    public string? CompanyTaxId { get; init; }
    public Notification<DomainError> Notification { get; } = new();

    public bool IsCompany => !string.IsNullOrEmpty(CompanyName);
    
    public bool Validate()
    {
        if (!string.IsNullOrEmpty(CompanyName) || !string.IsNullOrEmpty(CompanyTaxId) || CompanyAddress is not null)
        {
            if (string.IsNullOrEmpty(CompanyName) || string.IsNullOrEmpty(CompanyTaxId) || CompanyAddress is null || (CompanyAddress is not null && !CompanyAddress.Validate()))
                Notification.AddError(new() { Message = "Company has some empty or not valid fields.", Source = nameof(Client)});
        }

        if (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ||
            (CorrespondencyAddress is null || CorrespondencyAddress.Validate()) || !string.IsNullOrEmpty(IdNumber))
        {
            Notification.AddError(new() { Message = "Personal data of responsible person must be provided", Source = nameof(Client)});
        }

        if(string.IsNullOrEmpty(IdNumber))
            Notification.AddError(new() {Message = "Id number is required", Source = nameof(Client)});
        
        return !Notification.HasErrors;
    }

    
}