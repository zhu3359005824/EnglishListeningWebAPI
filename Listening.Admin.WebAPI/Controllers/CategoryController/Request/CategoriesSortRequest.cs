
using FluentValidation;

namespace Listening.Admin.WebAPI.Controllers.CategoryController.Request;
public class CategoriesSortRequest
{
    /// <summary>
    /// 排序后的类别Id
    /// </summary>
    public Guid[] SortedCategoryIds { get; set; }
}

public class CategoriesSortRequestValidator : AbstractValidator<CategoriesSortRequest>
{
    public CategoriesSortRequestValidator()
    {
        RuleFor(r => r.SortedCategoryIds).NotNull().NotEmpty();
    }
}