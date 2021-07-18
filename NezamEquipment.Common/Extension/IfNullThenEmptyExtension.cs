namespace NezamEquipment.Common.Extension
{
    public static class IfNullThenEmptyExtension
    {
        public static string IfNullThenEmpty(this string data, string then = "")
        {
            return !string.IsNullOrWhiteSpace(data) ? data : then;
        }
    }
}
