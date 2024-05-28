namespace BuberBreakfast.contracts.BreakfastResponse;

public record CreateBreakfastResponse(
  Guid Id,
  string Name,
  string Description,
  DateTime StartDateTime,
  DateTime EndDateTime,
  DateTime lastModifiedDateTime,
  List<string> Savory,
  List<string> Sweet
);