using System.Drawing;
using System.Globalization;

namespace SwColorChanger.Colors;

public class RandSatColor : IColor
{
   public required string BaseColor;
   public required int Deviation;
   
   public string Render()
   {
      var color = ColorHelpers.ToColor(BaseColor);
      var deviation = 1 + (Random.Shared.NextSingle() - 0.5f) * (Deviation / 255f);
      var newColor = Color.FromArgb(
         int.Clamp((int)(color.R * deviation), 0, 255), 
         int.Clamp((int)(color.G * deviation), 0, 255), 
         int.Clamp((int)(color.B * deviation), 0, 255));
      var rgbRepr = (newColor.ToArgb() & 0xFFFFFF).ToString("X");
      return rgbRepr;
   }
}