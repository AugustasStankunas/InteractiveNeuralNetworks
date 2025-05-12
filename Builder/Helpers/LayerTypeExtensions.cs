using System.ComponentModel;
using System.Reflection;
using Builder.Enums;

namespace Builder.Helpers
{
    public static class LayerTypeExtensions
    {
        public static string GetDescription(this LayerType value)
        {
            // Find the enum‐member field
            var fi = value
                .GetType()
                .GetField(value.ToString())
                ?? throw new InvalidOperationException($"Enum member '{value}' not found");

            // Look for [Description("…")] on it
            var attr = fi.GetCustomAttribute<DescriptionAttribute>();

            // If present, use it; otherwise fall back to the name
            return attr?.Description ?? value.ToString();
        }
    }
}
