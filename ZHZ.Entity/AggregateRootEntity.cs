using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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


        //忽略
        [NotMapped]
        private List<INotification> domainEvents = new();

        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents.Add(eventItem);
        }



        /// <summary>
        /// 检查并添加事件,如不存在该事件,则添加
        /// </summary>
        /// <param name="eventItem"></param>
        public void CheckAndAddDomainEvent(INotification eventItem)
        {
            if (!domainEvents.Contains(eventItem))
            {
                domainEvents.Add(eventItem);
            }
        }
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return domainEvents;
        }



        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeleteTime=DateTime.Now;
        }
    }
}
