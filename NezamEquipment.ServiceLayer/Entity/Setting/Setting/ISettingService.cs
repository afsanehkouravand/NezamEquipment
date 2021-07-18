using System.Threading.Tasks;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting
{
    public interface ISettingService
    {
        T Get<T>() where T : new ();
        Task<T> GetAsync<T>() where T : new ();
        Task<DbResult> ModifyAsync<T>(T data) where T : class;
    }
}