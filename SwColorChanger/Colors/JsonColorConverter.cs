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
      return ConvertToColor(root);
   }

   private IColor ConvertToColor(JsonElement root)
   {
      if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("$randSat", out JsonElement randSatElement))
      {
         return new RandSatColor
         {
            BaseColor = randSatElement.GetProperty("base").GetString()!,
            Deviation = randSatElement.GetProperty("deviation").GetInt32()
         };
      }

      if (root.ValueKind == JsonValueKind.Object &&
          root.TryGetProperty("$randFrom", out JsonElement randSetArray))
      {
         var colorSet = randSetArray.EnumerateArray().Select(el =>
            (ConvertToColor(el), el.TryGetProperty("probability", out var prob) ? prob.GetSingle() : 0)).ToArray();

         return new RandFromSetColor(colorSet);
      }

      return new FixedColor {Color = root.GetString()!};
   }

   public override void Write(Utf8JsonWriter writer, IColor value, JsonSerializerOptions options)
   {
      throw new InvalidOperationException();
   }
}