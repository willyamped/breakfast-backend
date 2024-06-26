using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;
public class Breakfast {
  public const int MinNameLength = 3;
  public const int MaxNameLength = 50;
  public const int MinDescriptionLength = 3;
  public const int MaxDescriptionLength = 150;

  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime StartDateTime { get; set; }
  public DateTime EndDateTime { get; set; }
  public DateTime LastModifiedDateTime { get; set; }
  public List<string> Savory { get; set; }
  public List<string> Sweet { get; set; }

  public Breakfast() {}
  public Breakfast(
    Guid id,
    string name,
    string description,
    DateTime startDateTime,
    DateTime endDateTime,
    DateTime lastModifiedDateTime,
    List<string> savory,
    List<string> sweet
  )
  {
    Id = id;
    Name = name;
    Description = description;
    StartDateTime = startDateTime;
    EndDateTime = endDateTime;
    LastModifiedDateTime = lastModifiedDateTime;
    Savory = savory;
    Sweet = sweet;
  }

  public static ErrorOr<Breakfast> Create(
    string name,
    string description,
    DateTime startDateTime,
    DateTime endDateTime,
    List<string> savory,
    List<string> sweet,
    Guid? id = null
  ) {
    List<Error> errors = new();
    if (name.Length is < MinNameLength or > MaxNameLength) {
      errors.Add(Errors.Breakfast.InvalidName);
    }
    if (description.Length is < MinNameLength or > MaxNameLength) {
      errors.Add(Errors.Breakfast.InvalidDescription);
    }
    if (errors.Count > 0) {
      return errors;
    }
    var breakfast = new Breakfast(
        id ?? Guid.NewGuid(),
        name,
        description,
        startDateTime,
        endDateTime,
        DateTime.UtcNow,
        savory,
        sweet
    );
    return breakfast;
  }
}