using System.Text.Json.Serialization;

namespace SwColorChanger.Colors;

public class FixedColor : IColor
{
   public required string Color;
   
   public string Render()
   {
      return ColorHelpers.ToUnifiedColor(Color);
   }
}