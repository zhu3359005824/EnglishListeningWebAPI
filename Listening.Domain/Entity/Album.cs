﻿using ZHZ.Entity;

namespace Listening.Domain.Entity
{
    /// <summary>
    /// 听力的列表,如4级级听力中包含2022年,2023年,2024年的听力
    /// </summary>
    public class Album : AggregateRootEntity
    {
        public Album(string albumName, int showIndex, string categoryName) : base()
        {
            AlbumName = albumName;
            ShowIndex = showIndex;

            CategoryName = categoryName;
        }

        public string AlbumName { get; set; }

        public int ShowIndex { get; private set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }





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
