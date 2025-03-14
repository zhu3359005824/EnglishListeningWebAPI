using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.Entity
{
    public class AggregateRootEntity:IAggregateRoot
    {
        public AggregateRootEntity()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }

 
        public Guid Id { get; init; }

        /// <summary>
        /// 软删除
        /// </summary>
        public bool IsDeleted { get; private set; }=false;

        public DateTime? CreateTime { get; init; }

        public DateTime? DeleteTime { get; private set; }

       

        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeleteTime=DateTime.Now;
        }
    }
}
