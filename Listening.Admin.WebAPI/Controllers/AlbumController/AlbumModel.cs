using Listening.Domain.Entity;

namespace Listening.Admin.WebAPI.Controllers.AlbumController
{
    public record AlbumModel(Guid id, string albumname, string categoryName, DateTime? createTime)
    {
        public static AlbumModel? Create(Album a)
        {
            if (a == null) throw new ArgumentNullException("Album不存在");

            return new AlbumModel(a.Id, a.AlbumName, a.CategoryName, a.CreateTime);
        }


        public static AlbumModel[] Create(Album[] items)
        {
            return items.Select(e => Create(e)!).ToArray();
        }
    }
}
