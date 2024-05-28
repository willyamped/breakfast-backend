using BuberBreakfast.contracts.BreakfastResponse;
using ErrorOr;
using BuberBreakfast.Models;

public interface IBreakfastService {
  ErrorOr<Created> CreateBreakfast(Breakfast breakfast);
  ErrorOr<Deleted> DeleteBreakfast(Guid id);
  ErrorOr<Breakfast> GetBreakfast(Guid id);
  ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast);
}