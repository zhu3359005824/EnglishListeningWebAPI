namespace IDentity.WebAPI
{
    public class UnitOfWorkAttribute:Attribute
    {
        public Type[] DbContextTypes {  get; init; }

        public UnitOfWorkAttribute( params Type[] dcContextType  ) 
        {
            DbContextTypes = dcContextType;
        }
    }
}
