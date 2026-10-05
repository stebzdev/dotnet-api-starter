using System.Text.Json;
using System.Text.Json.Serialization;
using Starter.Domain.ReportingEntities;

namespace Starter.Application.Common.Caching;

public sealed class ReportingEntityCodeJsonConverter : JsonConverter<ReportingEntityCode>
{
    public override ReportingEntityCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Reporting entity code cannot be null.");

        return new ReportingEntityCode(value);
    }

    public override void Write( Utf8JsonWriter writer, ReportingEntityCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
