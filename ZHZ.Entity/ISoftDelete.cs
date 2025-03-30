namespace ZHZ.Entity
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }

        void SoftDelete();
    }
}
