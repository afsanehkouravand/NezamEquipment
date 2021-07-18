using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Attribute
{
    public class AuthorizeAreaNameAttribute : FilterAttribute
    {
        public string Name;
        public AuthorizeAreaNameAttribute(string name = "")
        {
            Name = name;
        }
    }
}
