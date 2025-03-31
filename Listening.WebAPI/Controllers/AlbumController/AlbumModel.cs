using Listening.Domain.Entity;

namespace Listening.Main.WebAPI.Controllers.AlbumController
{
    public record AlbumModel(Guid id, string name)
    {
        public static AlbumModel? Create(Album a)
        {
            if (a == null) throw new ArgumentNullException("Album不存在");

            return new AlbumModel(a.Id, a.AlbumName);
        }


        public static AlbumModel[] Create(Album[] items)
        {
            return items.Select(e => Create(e)!).ToArray();
        }
    }
}
