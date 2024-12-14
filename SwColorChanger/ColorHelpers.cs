using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace SwColorChanger;

public static class ColorHelpers
{
   public static string ToUnifiedColor(string str)
   {
      return ColorToString(ToColor(str));
   }
   
   public static Color ToColor(string str)
   {
      if (str.Contains(','))
      {
         var rgb = str.Split(',').Select(int.Parse).ToArray();
         Debug.Assert(rgb.Length == 3);
         var color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
         return color;
      }

      return Color.FromArgb(int.Parse(str, NumberStyles.HexNumber));
   }

   private static string ColorToString(Color color) => (color.ToArgb() & 0xFFFFFF).ToString("X");
}