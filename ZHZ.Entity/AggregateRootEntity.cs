using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.Entity
{
    public class AggregateRootEntity: BaseEntity,IAggregateRoot,ISoftDelete
    {
        public AggregateRootEntity()
        {
            
            CreateTime = DateTime.Now;
        }

 
        

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
