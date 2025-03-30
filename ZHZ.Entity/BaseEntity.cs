using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHZ.Entity
{
    public class BaseEntity : IDomainEvents, IEntity
    {

        public Guid Id { get; init; } = Guid.NewGuid();


        //忽略
        [NotMapped]
        private List<INotification> domainEvents = new();
        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents.Add(eventItem);
        }

        public void AddDomainEventIfAbsent(INotification eventItem)
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
    }
}
