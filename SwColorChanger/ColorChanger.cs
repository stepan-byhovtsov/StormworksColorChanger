using System.Xml;
using SwColorChanger.Colors;

namespace SwColorChanger;

public static class ColorChanger
{
   public static void RecolorXmlDocument(XmlDocument xmlDocument, Dictionary<string, IColor> colorMap)
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
   }
   
   
   static string NormalizeColor(string? color)
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

   static string DenormalizeColor(string color)
   {
      color = color.TrimStart('0');
      return color.Length == 0 ? "x" : color;
   }
}