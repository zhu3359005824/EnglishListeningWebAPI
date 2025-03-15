using Listening.Domain.Entity;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    public record CategoryModel(Guid id, string name)
    {
        public static CategoryModel? Create(Category a)
        {
            if (a == null) throw new ArgumentNullException("Album不存在");

            return new CategoryModel(a.Id, a.CategoryName);
        }


        public static CategoryModel[] Create(Category[] items)
        {
            return items.Select(e => Create(e)!).ToArray();
        }
    }
}
