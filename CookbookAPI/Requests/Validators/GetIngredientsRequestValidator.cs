using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;
using CookbookAPI.Requests.Ingredients;
using CookbookAPI.Requests.Recipes;
using FluentValidation;

namespace CookbookAPI.Requests.Validators
{
    public class GetIngredientsRequestValidator : AbstractValidator<GetIngredientsRequest>
    {
        protected readonly int[] _allowedPageSizes = new[] { 5, 10, 15, 20, 25 };

        protected readonly string[] _allowedSortByColumnNames =
            {nameof(Ingredient.Name).ToLower(), nameof(Ingredient.Id).ToLower()};


        public GetIngredientsRequestValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!_allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", _allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || _allowedSortByColumnNames.Contains(value.ToLower()))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", _allowedSortByColumnNames)}]");

            RuleFor(x => x.SortDirection)
                .Must(value =>
                    string.IsNullOrEmpty(value) ||
                    (value.ToLower() == SortDirection.ASC || value.ToLower() == SortDirection.DESC))
                .WithMessage("Sort direction is optional, or must be asc or desc");
        }
    }
}
