using System.Text.Json.Serialization;

namespace SwColorChanger.Colors;

[JsonConverter(typeof(JsonColorConverter))]
public interface IColor
{
   public string Render(ComponentDescription cd);
}