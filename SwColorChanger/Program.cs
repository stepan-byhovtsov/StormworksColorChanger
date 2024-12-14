using System.Text.Json;
using System.Xml;
using SwColorChanger;
using SwColorChanger.Colors;

Dictionary<string, IColor>? colorMap = null;
string name = "colors";
while (colorMap == null)
{
   try
   {
      colorMap = JsonSerializer.Deserialize<Dictionary<string, IColor>>(File.ReadAllText($"{name}.json"),
         new JsonSerializerOptions {Converters = {new JsonColorConverter()}});
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
colorMap = colorMap!.Select(kv => new KeyValuePair<string, IColor>(ColorHelpers.ToUnifiedColor(kv.Key), kv.Value)).ToDictionary();

if (args.Length == 0)
{
   Console.Write("Please, specify vehicle to replace colors: ");
   args = [Console.ReadLine()!];
}

try
{
   var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Stormworks\data\vehicles", args[0] + ".xml");
   using var file = File.Open(path, FileMode.Open);
   var xmlDocument = new XmlDocument();
   xmlDocument.Load(file);
   try
   {
      var vehicle = xmlDocument["vehicle"]!;
      var bodies = vehicle["bodies"]!;
      foreach (XmlElement body in bodies)
      {
         var components = body["components"]!;
         foreach (XmlElement component in components)
         {
            var obj = component["o"]!;
            var ac = NormalizeColor(obj.Attributes["ac"]?.Value);
            var bc = NormalizeColor(obj.Attributes["bc"]?.Value);
            if (colorMap.TryGetValue(ac, out var replacementAc))
            {
               obj.SetAttribute("ac", replacementAc.Render());
            }
            if (colorMap.TryGetValue(bc, out var replacementBc))
            {
               obj.SetAttribute("bc", replacementBc.Render());
            }

            var sc = obj.Attributes["sc"];
            if (sc != null)
            {
               var colors = sc.Value.Split(',');
               if (colors.Length > 1)
               {
                  // [0] = count of colors
                  for (int i = 1; i < colors.Length; i++)
                  {
                     var color = NormalizeColor(colors[i]);
                     if (colorMap.TryGetValue(color, out var replacementSc))
                     {
                        colors[i] = DenormalizeColor(replacementSc.Render());
                     }
                  }

                  var newValue = string.Join(',', colors);
                  sc.Value = newValue;
               }
            }
            
            var gc = obj.Attributes["gc"];
            if (gc != null)
            {
               var colors = gc.Value.Split(',');
               if (colors.Length > 1)
               {
                  for (int i = 0; i < colors.Length; i++)
                  {
                     var color = NormalizeColor(colors[i]);
                     if (colorMap.TryGetValue(color, out var replacementSc))
                     {
                        colors[i] = DenormalizeColor(replacementSc.Render());
                     }
                  }

                  var newValue = string.Join(',', colors);
                  gc.Value = newValue;
               }
            }
         }
      }
      
      var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Stormworks\data\vehicles", args[0] + "_rc.xml");
      xmlDocument.Save(outputPath);
      
      Console.WriteLine("Successfully recolored.");
      WaitForExit();
      return 0;
   }
   catch
   {
      Console.WriteLine("Cannot parse vehicle file.");
      WaitForExit();
      return 1;
   }
}
catch
{
   Console.WriteLine($"Cannot open {args[0]} file.");
   WaitForExit();
   return 1;
}

string NormalizeColor(string? color)
{
   if (color == null)
   {
      return "0";
   }
   
   if (color == "x")
   {
      return "0";
   }

   return color;
}

string DenormalizeColor(string color)
{
   color = color.TrimStart('0');
   return color.Length == 0 ? "x" : color;
}

void WaitForExit()
{
   Console.WriteLine("Press any key to exit.");
   Console.ReadKey();
}