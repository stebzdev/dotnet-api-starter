using System.Text.Json;
using System.Text.Json.Serialization;
using Starter.Domain.SampleItems;

namespace Starter.Application.Common.Caching;

public sealed class SampleItemCodeJsonConverter : JsonConverter<SampleItemCode>
{
    public override SampleItemCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Sample item code cannot be null.");

        return new SampleItemCode(value);
    }

    public override void Write( Utf8JsonWriter writer, SampleItemCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
