using System;
using System.Web;
using System.Web.Script.Serialization;

namespace NezamEquipment.Web.Models
{
    public class LoginInfo
    {
        public string Username { get; set; }

        public Guid Id { get; set; }

        public static LoginInfo Get()
        {
            try
            {
                return (new JavaScriptSerializer()).Deserialize<LoginInfo>(HttpContext.Current.User.Identity.Name);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

}