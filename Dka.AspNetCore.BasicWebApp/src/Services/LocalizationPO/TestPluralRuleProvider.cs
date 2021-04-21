using System.Globalization;
using OrchardCore.Localization;

namespace Dka.AspNetCore.BasicWebApp.Services.LocalizationPO
{
    public class TestPluralRuleProvider : IPluralRuleProvider
    {
        public int Order => 1;
        
        public bool TryGetRule(CultureInfo culture, out PluralizationRuleDelegate rule)
        {
            rule = n => n switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                _ => 4
            };

            return true;
        }
    }
}