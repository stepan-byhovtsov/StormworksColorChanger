using System.Runtime.InteropServices;
using System.Text.Json;
using SwColorChanger.Colors;

namespace SwColorChanger;

public static class ColorMap
{
   public static Dictionary<string, IColor> ImportColorMap()
   {
      var name = "colors";
      var jsonSerializerOptions = new JsonSerializerOptions {Converters = {new JsonColorConverter()}};
      Dictionary<string, IColor>? colorMap = null;
      while (colorMap == null)
      {
         try
         {
            colorMap = JsonSerializer.Deserialize<Dictionary<string, IColor>>(File.ReadAllText($"{name}.json"),
               jsonSerializerOptions);
         }
         catch (FileNotFoundException)
         {
            Console.Write($"No {name}.json file found. Specify filename: ");
            name = Console.ReadLine()!;
         }
         catch (JsonException)
         {
            Console.Write($"Cannot parse {name}.json file. You can try again: ");
            name = Console.ReadLine()!;
         }
      }
      return colorMap.Select(kv => new KeyValuePair<string, IColor>(ColorHelpers.ToUnifiedColor(kv.Key), kv.Value)).ToDictionary();
   } 
}