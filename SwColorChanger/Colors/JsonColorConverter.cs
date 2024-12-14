using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SwColorChanger.Colors;

public class JsonColorConverter : JsonConverter<IColor>
{
   public override IColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
   {
      using var doc = JsonDocument.ParseValue(ref reader);
      var root = doc.RootElement;
      
      if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("$randSat", out JsonElement randSatElement))
      {
         return new RandSatColor
         {
            BaseColor = randSatElement.GetProperty("base").GetString()!,
            Deviation = randSatElement.GetProperty("deviation").GetInt32()
         };
      }
      else
      {
         return new FixedColor {Color = root.GetString()!};
      }
   }

   public override void Write(Utf8JsonWriter writer, IColor value, JsonSerializerOptions options)
   {
      throw new InvalidOperationException();
   }
}