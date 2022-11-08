using System.Text.Json.Serialization;
using System.Text.Json;
using TriageConfiguration.TriageElements;

namespace TriageConfigurationWeb
{
    public class TriageConfigConverter : JsonConverter<TriageConfig>
    {
        private readonly JsonSerializerOptions ConverterOptions;
        public TriageConfigConverter(JsonSerializerOptions converterOptions)
        {
            ConverterOptions = converterOptions;
        }
        public override TriageConfig? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TriageConfig>(ref reader, ConverterOptions);
        }

        public override void Write(Utf8JsonWriter writer, TriageConfig value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<TriageConfig>(writer, value, ConverterOptions);
        }
    }
}
