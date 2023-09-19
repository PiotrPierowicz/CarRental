using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.Sales.Application.Sagas.CarRentProcess;
using CarRental.Sales.Model;

namespace CarRental.Sales.Application.IntegrationEvents.Handlers;

public class ClientCheckedIntegrationEventHandler: IEventAsyncHandler<ClientChecked>
{
    private readonly CarRentProcessSagaManager _carRentProcessSagaManager;
    private readonly IEventBus _eventBus;
    private readonly IRentalRepository _rentalRepository;

    public ClientCheckedIntegrationEventHandler(CarRentProcessSagaManager carRentProcessSagaManager, IEventBus eventBus, IRentalRepository rentalRepository)
    {
        _carRentProcessSagaManager = carRentProcessSagaManager;
        _eventBus = eventBus;
        _rentalRepository = rentalRepository;
    }

    public async Task HandleAsync(ClientChecked @event)
    {
        await _carRentProcessSagaManager.ChangeStateAsync(CarRentProcessState.CLIENT_CHECKED, @event.RentNumber,
            @event.CorrelationId);

        var rental = await _rentalRepository.GetRental(@event.RentNumber);
        var confirmedRent = rental.ConfirmRent(@event.RentNumber);
        
        await _rentalRepository.UnitOfWork.SaveChangesAsync();

        RentConfirmed rentConfirmedEvent = new()
        {
            RentNumber = @event.RentNumber,
            ClientId = @event.ClientId,
            CorrelationId = @event.CorrelationId,
            FinalPrice = confirmedRent.FinalPrice,
            CorrespondencyAddress = confirmedRent.Client.CorrespondencyAddress,
            IsCompany = confirmedRent.Client.IsCompany,
            CompanyTaxId = confirmedRent.Client.CompanyTaxId,
            CompanyName = confirmedRent.Client.CompanyName,
            CompanyAddress = confirmedRent.Client.CompanyAddress,
            ClientFirstName = confirmedRent.Client.FirstName,
            ClientLastName = confirmedRent.Client.LastName
        };
    }
}