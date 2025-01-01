using System.Diagnostics.CodeAnalysis;
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

   [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: Entry[System.ValueTuple`3[System.Int32,System.Int32,System.Int32],SwColorChanger.Colors.SpotOptions][]; size: 354MB")]
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

      if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("$spotty", out JsonElement spottyElement))
      {
         return new SpottyColor(
            baseColor: ConvertToColor(spottyElement.GetProperty("base")),
            spots: spottyElement.GetProperty("spots").EnumerateArray().Select(
               el => new SpotOptions(el.GetProperty("radius").GetSingle(), el.GetProperty("density").GetInt32(),
                  ConvertToColor(el.GetProperty("color")))).ToArray());
      }

      if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("$perlin", out JsonElement perlinElement))
      {
         return new PerlinColor(TryGetProperty(perlinElement, "seed")?.GetInt32() ?? Random.Shared.Next())
         {
            A = ConvertToColor(perlinElement.GetProperty("a")),
            B = ConvertToColor(perlinElement.GetProperty("b")),
            Div = TryGetProperty(perlinElement, "div")?.GetSingle() ?? 8,
            Off = TryGetProperty(perlinElement, "off")?.GetSingle() ?? 0,
            EasingMode = TryGetProperty(perlinElement, "easing")?.GetString() ?? "quintic",
            Octaves = TryGetProperty(perlinElement, "octaves")?.GetInt32() ?? 16
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

   private JsonElement? TryGetProperty(JsonElement el, string property)
   {
      if (el.TryGetProperty(property, out var res))
      {
         return res;
      }

      return null;
   }
   
   public override void Write(Utf8JsonWriter writer, IColor value, JsonSerializerOptions options)
   {
      throw new InvalidOperationException();
   }
}