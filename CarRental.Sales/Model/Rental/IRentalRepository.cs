using CarRental.BuildingBlocks.DDD;

namespace CarRental.Sales.Model;

public interface IRentalRepository: IRepository<Rental>
{
    Task<Rental> GetRental(string rentalIdentifier);
}