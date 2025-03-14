using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Entity;

namespace Listening.Domain.Entity
{
    /// <summary>
    /// 听力的分类,如4级,6级
    /// </summary>
    public class Category:AggregateRootEntity
    {
        public Category( string categoryName, int showIndex) : base()
        {
            CategoryName = categoryName;
            ShowIndex = showIndex;
           
        }

        public string CategoryName { get;set; }

        /// <summary>
        /// 显示顺序 
        /// </summary>
        public int ShowIndex { get; private set; }
       

        /// <summary>
        /// 封面图片。现在一般都不会直接把图片保存到数据库中（Blob），而是只是保存图片的路径。
        /// </summary>
        public Uri? CoverUrl { get; private set; }


        public Category SetCoverUrl(Uri value)
        {
            this.CoverUrl = value;
            return this;
        }


        public Category ChangeShowIndex(int value)
        {
            this.ShowIndex = value;
            return this;
        }

        public Category ChangeCategoryName(string name)
        {
            this.CategoryName = name;
            return this;
        }

        public Category ChangeCoverUrl(Uri value)
        {
            //todo: 做项目的时候，不管这个事件是否有被用到，都尽量publish。
            this.CoverUrl = value;
            return this;
        }

    }
}
