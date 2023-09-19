using CarRental.BuildingBlocks.ServiceIntegration;

namespace CarRental.Sales.Application.IntegrationEvents;

public record ClientChecked: IEvent
{
    public required string RentNumber { get; init; }
    public required int ClientId { get; init; }
    public Guid CorrelationId { get; set; }
    public DateTime Timestamp { get; set; } = new DateTime();
    public string EventName => nameof(ClientChecked);
}