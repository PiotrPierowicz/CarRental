using CarRental.BuildingBlocks.DDD;
using CarRental.Sales.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Sales.Application.Sagas.CarRentProcess;

public sealed class CarRentProcessSagaManager
{
    private readonly SalesContext _ctx;

    public CarRentProcessSagaManager(SalesContext ctx)
    {
        _ctx = ctx;
    }

    public void Start(string rentNumber, Guid correlationId)
    {
        _ctx.CarRentProcessStates.Add(new CarRentProcessState()
        {
            CurrentState = CarRentProcessState.CHECKING_THE_CLIENT,
            ChangedDate = DateTime.Now,
            RentNumber = rentNumber,
            CorrelationId = correlationId
        });
    }

    public async Task ChangeStateAsync(string newState, string rentNumber, Guid correlationId, bool saveContextChanges = false)
    {
        var state = await _ctx.CarRentProcessStates.SingleOrDefaultAsync(x => x.RentNumber.Equals(rentNumber) && x.CorrelationId.Equals(correlationId));

        if (state is null)
            throw new DomainException($"No rent with number {rentNumber} process started");

        state.CurrentState = newState;
        state.ChangedDate = DateTime.Now;

        if(saveContextChanges) await _ctx.SaveChangesAsync();
    }
}