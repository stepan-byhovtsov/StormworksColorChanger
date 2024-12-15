﻿using System.Xml;
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
            UpdateTag(colorMap, obj, "ac");
            UpdateTag(colorMap, obj, "bc");
            UpdateTag(colorMap, obj, "bc2");
            UpdateTag(colorMap, obj, "bc3");

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
               else
               {
                  Array.Resize(ref colors, int.Parse(colors[0])+1);
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

   static void UpdateTag(Dictionary<string, IColor> colorMap, XmlElement obj, string attr)
   {
      var ac = NormalizeColor(obj.Attributes[attr]?.Value);
      if (colorMap.TryGetValue(ac, out var replacementAc))
      {
         obj.SetAttribute(attr, replacementAc.Render());
      }
   }
   
   
   static string NormalizeColor(string? color)
   {
      if (color == null)
      {
         return "FFFFFF";
      }
   
      if (color == "x")
      {
         return "FFFFFF";
      }

      return color;
   }

   static string DenormalizeColor(string color)
   {
      color = color.TrimStart('0');
      return color == "FFFFFF" ? "x" : color;
   }
}