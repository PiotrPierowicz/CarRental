namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEventAsyncHandler<T> where T: IEvent
{
    Task HandleAsync(T @event);
}