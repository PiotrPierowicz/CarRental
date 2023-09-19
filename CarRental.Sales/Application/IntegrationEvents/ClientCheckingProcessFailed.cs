using CarRental.BuildingBlocks.ServiceIntegration;

namespace CarRental.Sales.Application.IntegrationEvents;

public record ClientCheckingProcessFailed : IEvent
{
    public required string IdNumber { get; init; }
    public string? CompanyTaxId { get; init; }
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public string EventName => nameof(ClientCheckingProcessFailed);
    
}