namespace NezamEquipment.Common.Normalization
{
    public static class PersianNormalizationExtension
    {
        public static string ApplyModeratePersianRules(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            if (!data.ContainsFarsi())
                return data;

            return data
                .ApplyCorrectYeKe()
                .ApplyHalfSpaceRule()
                .YeHeHalfSpace()
                .CleanUpZwnj()
                .CleanupExtraMarks();
        }

        public static string ApplyModeratePersianRulesForUserName(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            if (!data.ContainsFarsi())
                return data;

            var friendlyName = data
                .ApplyCorrectYeKe()
                .RemoveDiacritics()
                .CleanUnderLines()
                .RemovePunctuation()
                .CleanupExtraMarks();

            return friendlyName.Trim();
        }
    }
}
