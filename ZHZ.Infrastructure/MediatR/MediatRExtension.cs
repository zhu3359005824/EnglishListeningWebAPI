using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Entity;

namespace ZHZ.Infrastructure.MediatR
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            return services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblies(assemblies.ToArray());
            });

        }

        /// <summary>
        /// 发出事件
        /// </summary>
        /// <param name=""></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventAsync(this IMediator mediator,DbContext ctx)
        {
            var domainEntity=ctx.ChangeTracker.Entries<IDomainEvents>().Where(x=>x.Entity.GetDomainEvents().Any());

            var domainEvent=domainEntity.SelectMany(x=>x.Entity.GetDomainEvents().ToList());

            domainEntity.ToList().ForEach(e=>e.Entity.ClearDomainEvents());

            foreach(var item in domainEvent)
            {
                await mediator.Publish(item);
            }
        }
    }
}
