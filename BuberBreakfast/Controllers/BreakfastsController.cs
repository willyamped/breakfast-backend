using BuberBreakfast.contracts.BreakfastResponse;
using BuberBreakfast.ServiceErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Models;
using Microsoft.AspNetCore.Authorization;

namespace BuberBreakfast.Controllers;
public class BreakfastsController : ApiController {
  private readonly IBreakfastService _breakfastService;
  public BreakfastsController(IBreakfastService breakfastService) {
    _breakfastService = breakfastService;
  }

  [Authorize]
  [HttpPost]
  public async Task<IActionResult> CreateBreakfast(CreateBreakfastRequest request) {
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
    var createdBreakfastResult = await _breakfastService.CreateBreakfast(breakfast);

      return createdBreakfastResult.Match(
        created => CreatedAsGetBreakfast(breakfast),
        errors => Problem(errors)
      );
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetBreakfast(Guid id) {
    var getBreakfastResult = await _breakfastService.GetBreakfast(id);
    return getBreakfastResult.Match(
      breakfast => Ok(MapBreakfastResponse(breakfast)),
      errors => Problem(errors)
    );
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpsertBreakfast(Guid id, UpsertBreakfastRequest request) {
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
    var upsertBreakfastResult = await _breakfastService.UpsertBreakfast(breakfast);

    return upsertBreakfastResult.Match(
      upserted => upserted.IsNewlyCreated ? CreatedAsGetBreakfast(breakfast) : NoContent(),
      errors => Problem(errors)
    );
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteBreakfast(Guid id) {
    var upsertDeleteResult = await _breakfastService.DeleteBreakfast(id);

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