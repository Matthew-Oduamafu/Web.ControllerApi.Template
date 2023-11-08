using System.Text.Json;
using System.Text.Json.Serialization;

namespace Web.ControllerApi.Template.Extensions;

public static class SerializationExtension
{
    public static string ToJson<T>(this T obj, bool prettify = true) where T : class
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true, // For case-insensitive property names
            WriteIndented = prettify, // For pretty-printed JSON
            // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignore null values when writing JSON
            // Add other options as needed,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            AllowTrailingCommas = true
        };

        return JsonSerializer.Serialize(obj, options);
    }
}