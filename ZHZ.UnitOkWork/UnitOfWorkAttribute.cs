using Microsoft.EntityFrameworkCore;

namespace ZHZ.UnitOkWork
{
    [AttributeUsage(AttributeTargets.Class |AttributeTargets.Method,
            AllowMultiple = false)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; init; }

        public UnitOfWorkAttribute(params Type[] dcContextType)
        {
            DbContextTypes = dcContextType;
            foreach (var type in DbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"{type} must inherit from DbContext");
                }
            }
        }
    }
}
