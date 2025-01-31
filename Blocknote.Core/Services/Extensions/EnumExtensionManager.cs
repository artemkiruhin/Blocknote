using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blocknote.Core.Services.Extensions
{
    public static class EnumExtensionManager
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetField(value.ToString())?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? value.ToString();
        }

    }
}
