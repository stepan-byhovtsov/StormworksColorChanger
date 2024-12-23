using System.Runtime.InteropServices;
using System.Text.Json;
using SwColorChanger.Colors;

namespace SwColorChanger;

public static class ColorMap
{
   public static Dictionary<string, IColor> ImportColorMap(out string colorMapName)
   {
      var name = "colors";
      var jsonSerializerOptions = new JsonSerializerOptions {Converters = {new JsonColorConverter()}};
      Dictionary<string, IColor>? colorMap = null;
      FileInfo? file = null;
      while (colorMap == null || file == null)
      {
         file = new FileInfo($"{name}.json");
         try
         {
            colorMap = JsonSerializer.Deserialize<Dictionary<string, IColor>>(File.ReadAllText(file.FullName),
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

      colorMapName = Path.GetFileNameWithoutExtension(file.Name);
      return colorMap.Select(kv => new KeyValuePair<string, IColor>(ColorHelpers.ToUnifiedColor(kv.Key), kv.Value)).ToDictionary();
   } 
}