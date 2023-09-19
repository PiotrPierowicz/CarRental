using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.Sales.Model;

namespace CarRental.Sales.Application.IntegrationEvents;

public record RentConfirmed : IEvent
{
    public required string RentNumber { get; init; }
    public required int ClientId { get; init; }
    public decimal FinalPrice { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Address CorrespondencyAddress { get; init; }
    public required string IdNumber { get; init; }
    public string? CompanyName { get; init; }
    public Address? CompanyAddress { get; init; }
    public string? CompanyTaxId { get; init; }
    
    public Guid CorrelationId { get; set; }
    public DateTime Timestamp { get; set; } = new DateTime();
    public string EventName => nameof(ClientChecked);
}