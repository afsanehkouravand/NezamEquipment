using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Attribute
{
    public class AuthorizeControllerNameAttribute : FilterAttribute
    {
        public string Name;
        public new string Order;

        public AuthorizeControllerNameAttribute(string name = "", string order = "")
        {
            Name = name;
            Order = order;
        }

    }
}
