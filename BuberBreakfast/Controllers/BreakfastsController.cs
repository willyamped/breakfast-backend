using BuberBreakfast.contracts.BreakfastResponse;
using BuberBreakfast.ServiceErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Models;

namespace BuberBreakfast.Controllers;
public class BreakfastsController : ApiController {
  private readonly IBreakfastService _breakfastService;
  public BreakfastsController(IBreakfastService breakfastService) {
    _breakfastService = breakfastService;
  }

  [HttpPost]
  public IActionResult CreateBreakfast(CreateBreakfastRequest request) {
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.Create(
      request.Name,
      request.Description,
      request.StartDateTime,
      request.EndDateTime,
      request.Savory,
      request.Sweet 
    );

    if (requestToBreakfastResult.IsError) {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<Created> createdBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

      return createdBreakfastResult.Match(
        created => CreatedAsGetBreakfast(breakfast),
        errors => Problem(errors)
      );
  }

  [HttpGet("{id:guid}")]
  public IActionResult GetBreakfast(Guid id) {
    ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
    return getBreakfastResult.Match(
      breakfast => Ok(MapBreakfastResponse(breakfast)),
      errors => Problem(errors)
    );
  }

  [HttpPut("{id:guid}")]
  public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request) {
    var requestToBreakfastResult = Breakfast.Create(
      request.Name,
      request.Description,
      request.StartDateTime,
      request.EndDateTime,
      request.Savory,
      request.Sweet,
      id
    );

    if (requestToBreakfastResult.IsError) {
      return Problem(requestToBreakfastResult.Errors);
    }

    var breakfast = requestToBreakfastResult.Value;
    ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

    return upsertBreakfastResult.Match(
      upserted => upserted.IsNewlyCreated ? CreatedAsGetBreakfast(breakfast) : NoContent(),
      errors => Problem(errors)
    );
  }

  [HttpDelete("{id:guid}")]
  public IActionResult DeleteBreakfast(Guid id) {
    ErrorOr<Deleted> upsertDeleteResult = _breakfastService.DeleteBreakfast(id);

    return upsertDeleteResult.Match(
      deleted => NoContent(),
      errors => Problem(errors)
    );
  }

  private static CreateBreakfastResponse MapBreakfastResponse(Breakfast breakfast) {
    return new CreateBreakfastResponse(
      breakfast.Id,
      breakfast.Name,
      breakfast.Description,
      breakfast.StartDateTime,
      breakfast.EndDateTime,
      breakfast.LastModifiedDateTime,
      breakfast.Savory,
      breakfast.Sweet
    );
  }

  private CreatedAtActionResult CreatedAsGetBreakfast(Breakfast breakfast) {
    return CreatedAtAction(
      actionName: nameof(GetBreakfast),
      routeValues: new { id = breakfast.Id },
      value: MapBreakfastResponse(breakfast)
    );
  }
}