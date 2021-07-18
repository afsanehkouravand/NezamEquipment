namespace MyCommon.Helpers.Security
{
    internal class RoleAccessDto
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool HaveAccess { get; set; }
    }
}
