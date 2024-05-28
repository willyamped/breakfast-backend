using BuberBreakfast.contracts.BreakfastResponse;
using ErrorOr;
using BuberBreakfast.Models;

public interface IBreakfastService {
  Task<ErrorOr<Created>> CreateBreakfast(Breakfast breakfast);
  Task<ErrorOr<Deleted>> DeleteBreakfast(Guid id);
  Task<ErrorOr<Breakfast>> GetBreakfast(Guid id);
  Task<ErrorOr<UpsertedBreakfast>> UpsertBreakfast(Breakfast breakfast);
}