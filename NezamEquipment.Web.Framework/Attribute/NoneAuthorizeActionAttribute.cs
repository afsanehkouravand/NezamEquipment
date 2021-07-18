using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Attribute
{
    public class NoneAuthorizeActionAttribute : FilterAttribute
    {
        public string Name;

        public NoneAuthorizeActionAttribute(string name = "")
        {
            Name = name;
        }

    }
}
