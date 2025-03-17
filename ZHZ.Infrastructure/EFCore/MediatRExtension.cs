using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.Infrastructure.EFCore
{
    public static class MediatRExtension
    {

        //private readonly IMediator 


        /// <summary>
        /// 注册所有MediatR
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatR(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            return services.AddMediatR(assemblies.ToArray());
        }
        //public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
        //{
        //    var domainEntities = ctx.ChangeTracker
        //        .Entries<IDomainEvents>()
        //        .Where(x => x.Entity.GetDomainEvents().Any());

        //    var domainEvents = domainEntities
        //        .SelectMany(x => x.Entity.GetDomainEvents())
        //        .ToList();//加ToList()是为立即加载，否则会延迟执行，到foreach的时候已经被ClearDomainEvents()了

        //    domainEntities.ToList()
        //        .ForEach(entity => entity.Entity.ClearDomainEvents());

        //    foreach (var domainEvent in domainEvents)
        //    {
        //        await mediator.Publish(domainEvent);
        //    }
        //}
    }
}
