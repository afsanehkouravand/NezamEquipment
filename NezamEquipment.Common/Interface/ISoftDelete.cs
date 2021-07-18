namespace NezamEquipment.Common.Interface
{
    public interface ISoftDelete
    {
        /// <summary>
        /// حذف منطقی
        /// </summary>
        bool IsDeleted { get; set; }
    }
}