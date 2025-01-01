using System.Text.Json.Serialization;

namespace SwColorChanger.Colors;

public class FixedColor : IColor
{
   public required string Color;
   
   public string Render(ComponentDescription c)
   {
      return ColorHelpers.ToUnifiedColor(Color);
   }
}