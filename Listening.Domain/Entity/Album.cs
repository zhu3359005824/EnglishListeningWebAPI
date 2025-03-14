using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Entity;

namespace Listening.Domain.Entity
{
    /// <summary>
    /// 听力的列表,如4级级听力中包含2022年,2023年,2024年的听力
    /// </summary>
    public class Album:AggregateRootEntity
    {
        public Album(string albumName, int showIndex, Guid categoryId) : base()
        {
            AlbumName = albumName;
            ShowIndex = showIndex;
            CategoryId = categoryId;
        }

        public string AlbumName { get; set; }

        public int ShowIndex { get; private set; }

        public Guid CategoryId { get; set; }



        public Album ChangeShowIndex(int value)
        {
            this.ShowIndex = value;
            return this;
        }

        public Album ChangeAlbumName(string name)
        {
            this.AlbumName = name;
            return this;
        }

       



    }
}
