namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEventHandler<T> where T: IEvent
{
    void Handle(T @event);
}