using System.Runtime.InteropServices.JavaScript;

namespace SwColorChanger.Colors;

public class PerlinColor : IColor
{
   private readonly Dictionary<string, Func<float, float>> _interpolationModes = new()
   {
      {"linear", t => t},
      {"quintic", t => t * t * t * (t * (t * 6 - 15) + 10)},
      {"p1", t => t},
      {"p2", t => t*t},
      {"p3", t => t*t*t},
      {"p4", t => t*t*t*t},
      {"expo10", x => x == 0
         ? 0
         : Math.Abs(x - 1) < 0.0001
            ? 1
            : x < 0.5 ? MathF.Pow(2, 20 * x - 10) / 2
               : (2 - MathF.Pow(2, -20 * x + 10)) / 2
      },
      {"expo10In", x => x == 0 ? 0 : MathF.Pow(2, 10 * x - 10)},
      {"c1", t => t < 0.5f ? 0 : 1},
      {"c2", t => t < 0.55f ? 0 : 1},
   };
   
   public required IColor A;
   public required IColor B;
   public required float Div;
   public required float Off;
   public required int Octaves;
   public required string EasingMode;

   private Perlin3d _perlin;

   public PerlinColor(int seed)
   {
      _perlin = new Perlin3d(seed);
   }
   
   public string Render(ComponentDescription c)
   {
      var factor = _perlin.Noise(c.X / Div + Off, c.Y /  Div + Off, c.Z / Div + Off, Octaves) / 2 + 0.5f;
      var a = ColorHelpers.ToColor(A.Render(c));
      var b = ColorHelpers.ToColor(B.Render(c));
      var res = Perlin3d.Lerp(a, b, _interpolationModes[EasingMode](factor));
      return (res.ToArgb() & 0xFFFFFF).ToString("X");
   }
}