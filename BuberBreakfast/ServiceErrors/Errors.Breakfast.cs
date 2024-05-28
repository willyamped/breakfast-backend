using ErrorOr;

namespace BuberBreakfast.ServiceErrors
{
    public static class Errors
    {
        public static class Breakfast
        {
            public static Error InvalidName => Error.NotFound(
                code: "Breakfast.InvalidName",
                description: $"Breakfast name must be at least {Models.Breakfast.MinNameLength} and at most {Models.Breakfast.MaxNameLength} characters long"
            );

            public static Error InvalidDescription => Error.NotFound(
                code: "Breakfast.InvalidDescription",
                description: $"Breakfast description must be at least {Models.Breakfast.MinDescriptionLength} and at most {Models.Breakfast.MaxDescriptionLength} characters long"
            );
            public static Error NotFound => Error.NotFound(
                code: "Breakfast.NotFound",
                description: "Breakfast Not Found"
            );
        }
    }
}
