namespace MediaEncoder.Domain
{
    public enum ItemStatus
    {
        /// <summary>
        /// 准备
        /// </summary>
        Prepared,
        /// <summary>
        /// 进行中
        /// </summary>
        Running,

        Finish,
        Failed
    }
}