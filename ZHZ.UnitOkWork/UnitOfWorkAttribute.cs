using Microsoft.EntityFrameworkCore;

namespace ZHZ.UnitOkWork
{
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
