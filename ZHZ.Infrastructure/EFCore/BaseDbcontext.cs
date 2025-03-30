using MediatR;
using Microsoft.EntityFrameworkCore;
using ZHZ.Entity;
using ZHZ.Infrastructure.MediatR;

namespace ZHZ.Infrastructure.EFCore
{
    /// <summary>
    /// 具有事件的服务才实现
    /// </summary>
    public class BaseDbcontext : DbContext
    {


        private IMediator? _mediator;


        public BaseDbcontext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this._mediator = mediator;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (this._mediator != null)
            {
                //发出事件
                await _mediator.DispatchDomainEventAsync(this);

            }
            //获取所有已经软删除的实体
            var softDeletedEntity = this.ChangeTracker.Entries<ISoftDelete>().Where(t => t.State == EntityState.Modified && t.Entity.IsDeleted).Select(t => t.Entity).ToList();

            var result = await base.SaveChangesAsync(cancellationToken);


            //把被软删除的对象从Cache删除，否则FindAsync()还能根据Id获取到这条数据
            //因为FindAsync如果能从本地Cache找到，就不会去数据库上查询，而从本地Cache找的过程中不会管QueryFilter
            //就会造成已经软删除的数据仍然能够通过FindAsync查到的问题，因此这里把对应跟踪对象的state改为Detached，就会从缓存中删除了
            softDeletedEntity.ForEach(t => this.Entry(t).State = EntityState.Detached);

            return result;
        }



    }
}
