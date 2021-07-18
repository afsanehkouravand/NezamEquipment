using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NezamEquipment.Common.Security;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting
{
    public class SettingService : ISettingService
    {
        private readonly IDbSet<DomainClasses.Entity.Setting.Setting> _settings;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;

        public SettingService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _settings = _unitOfWork.Set<DomainClasses.Entity.Setting.Setting>();
        }

        public T Get<T>() where T : new()
        {
            var type = typeof(T);
            var settings = _settings.AsNoTracking().Where(x => x.Section == type.Name).Cacheable().ToList();
            return PrivateGet<T>(settings);
        }

        public async Task<T> GetAsync<T>() where T : new()
        {
            var type = typeof(T);
            var settings = await _settings.AsNoTracking().Where(x => x.Section == type.Name).Cacheable().ToListAsync();
            return PrivateGet<T>(settings);
        }

        private static T PrivateGet<T>(List<DomainClasses.Entity.Setting.Setting> settings) where T : new()
        {
            var newType = new T();
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (settings.Any())
            {
                foreach (var propertyInfo in props)
                {
                    var fieldName = propertyInfo.Name;
                    var prop = props.FirstOrDefault(x => x.Name.ToLower() == fieldName.ToLower());
                    if (prop != null)
                    {
                        var value = settings.Where(x => x.Key == fieldName).Select(x => x.Value).FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            prop.SetValue(newType, prop.GetValue(newType), null);
                        }
                        else
                        {
                            if (prop.PropertyType == typeof(int))
                            {
                                prop.SetValue(newType, int.Parse(value), null);
                            }
                            else if (prop.PropertyType == typeof(long))
                            {
                                prop.SetValue(newType, long.Parse(value), null);
                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                prop.SetValue(newType, bool.Parse(value), null);
                            }
                            else if (prop.PropertyType == typeof(long))
                            {
                                prop.SetValue(newType, long.Parse(value), null);
                            }
                            else
                            {
                                prop.SetValue(newType, value, null);
                            }
                        }
                    }
                }
            }

            var listEncryptAttribute = GetPropertiesWithDataTypePassword<T>();
            foreach (var prop in newType.GetType().GetProperties())
            {
                if (!listEncryptAttribute.Contains(prop.Name)) continue;

                var value = prop.GetValue(newType).ToString();
                if (string.IsNullOrWhiteSpace(value)) continue;

                prop.SetValue(newType, StringCipher.Decrypt(value, ConfigurationManager.AppSettings["Encrypt"]));
            }

            return newType;
        }

        public async Task<DbResult> ModifyAsync<T>(T data) where T : class
        {
            var listEncryptAttribute = GetPropertiesWithDataTypePassword<T>();
            ChangeValueOfDataTypePasswordProperties(data, listEncryptAttribute);

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var settings = await _settings.Where(x => x.Section == type.Name).ToListAsync();
            if (settings.Any())
            {
                foreach (var propertyInfo in props)
                {
                    var setting = settings.FirstOrDefault(x => x.Key == propertyInfo.Name);
                    if (setting != null)
                    {
                        var d = propertyInfo.GetValue(data);
                        var newValue = d?.ToString() ?? string.Empty;
                        setting.Value = newValue;
                        _unitOfWork.MarkAsChanged(setting);
                    }
                    else
                    {
                        var d = propertyInfo.GetValue(data);
                        var value = d?.ToString() ?? string.Empty;
                        var newSetting = new DomainClasses.Entity.Setting.Setting()
                        {
                            Key = propertyInfo.Name,
                            Section = type.Name,
                            Value = value,
                            CreatedOn = DateTime.Now,
                        };
                        _settings.Add(newSetting);
                    }
                }
            }
            else
            {
                foreach (var propertyInfo in props)
                {
                    var setting = settings.FirstOrDefault(x => x.Key == propertyInfo.Name);
                    if (setting == null)
                    {
                        var d = propertyInfo.GetValue(data);
                        var value = d?.ToString() ?? string.Empty;
                        var newSetting = new DomainClasses.Entity.Setting.Setting()
                        {
                            Key = propertyInfo.Name,
                            Section = type.Name,
                            Value = value,
                            CreatedOn = DateTime.Now,
                        };
                        _settings.Add(newSetting);
                    }
                }
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult(true);
        }

        #region Private

        private static void ChangeValueOfDataTypePasswordProperties<T>(T data, List<string> listEncryptAttribute)
        {
            foreach (var prop in data.GetType().GetProperties())
            {
                if (!listEncryptAttribute.Contains(prop.Name)) continue;

                var value = prop.GetValue(data).ToString();
                if (string.IsNullOrWhiteSpace(value)) continue;

                prop.SetValue(data, StringCipher.Encrypt(value, ConfigurationManager.AppSettings["Encrypt"]));
            }
        }

        private static List<string> GetPropertiesWithDataTypePassword<T>()
        {
            var listEncryptAttribute = (from property in typeof(T).GetProperties()
                let encrypt = (DataTypeAttribute) Attribute.GetCustomAttribute(property, typeof(DataTypeAttribute))
                where encrypt != null
                where encrypt.DataType == DataType.Password
                select property.Name).ToList();
            return listEncryptAttribute;
        }

        #endregion

    }
}
