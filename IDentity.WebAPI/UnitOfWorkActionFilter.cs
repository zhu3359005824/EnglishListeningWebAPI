using IDentity.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IDentity.WebAPI
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var result=await next();
            if (result.Exception != null)
            { 
                return;
            }

            var actionDesc=  context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDesc==null)
            {
                return;
            }

            //获取有UnitOfWork标记的方法
            var hasUnitOfWork=actionDesc.MethodInfo.GetCustomAttributes(typeof(UnitOfWorkAttribute),true).FirstOrDefault() as UnitOfWorkAttribute;

            if (hasUnitOfWork == null) { return; }

            foreach (var dbType in hasUnitOfWork.DbContextTypes)
            {
                var db=context.HttpContext.RequestServices.GetService(dbType) as MyIdentityDbContext;

                if (db==null)
                {
                    throw new InvalidOperationException($"Not Found {db}");
                }

                await db.SaveChangesAsync();
            }

        }
    }
}
