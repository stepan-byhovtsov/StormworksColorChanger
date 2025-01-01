using System.Xml;

namespace SwColorChanger;

public class ComponentDescription
{
   public int X;
   public int Y;
   public int Z;

   public ComponentDescription(XmlElement component)
   {
      var obj = component["o"]!;

      var vp = obj["vp"];

      if (vp == null)
      {
         X = 0;
         Y = 0;
         Z = 0;
      }
      else
      {
         X = int.Parse(vp.Attributes["x"]?.Value ?? "0");
         Y = int.Parse(vp.Attributes["y"]?.Value ?? "0");
         Z = int.Parse(vp.Attributes["z"]?.Value ?? "0");
      }
   }
}