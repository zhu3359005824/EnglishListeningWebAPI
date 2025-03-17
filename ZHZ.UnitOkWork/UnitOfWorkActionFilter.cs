
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace ZHZ.UnitOkWork
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var result=  await next();

            var actionDesc = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDesc == null)
            {
                return;
            }

            //获取有UnitOfWork标记的方法
            var hasUnitOfWork = actionDesc.MethodInfo.GetCustomAttributes(typeof(UnitOfWorkAttribute), true).FirstOrDefault() as UnitOfWorkAttribute;

           
            if (hasUnitOfWork == null)
            {
                
                return;
            }
            using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
            List<DbContext> dbCtxs = new List<DbContext>();
            foreach (var dbCtxType in hasUnitOfWork.DbContextTypes)
            {
                //用HttpContext的RequestServices
                //确保获取的是和请求相关的Scope实例
                var sp = context.HttpContext.RequestServices;
                DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
                dbCtxs.Add(dbCtx);
            }


            
            if (result.Exception == null)
            {
                foreach (var dbCtx in dbCtxs)
                {
                    await dbCtx.SaveChangesAsync();
                }
                txScope.Complete();
            }




        }
    }
}
