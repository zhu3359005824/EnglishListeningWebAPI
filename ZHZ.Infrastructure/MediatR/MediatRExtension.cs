using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.Infrastructure.MediatR
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            return services.AddMediatR(assemblies.ToArray());

        }

        public static async Task DispatchDomainEventAsync(this IMediator,DbContext ctx)
        {

        }
    }
}
