namespace SwColorChanger.Colors;

public class SpottyColor : IColor
{
   public IColor BaseColor { get; set; }
   public SpotOptions[] Spots { get; set; }
   private Dictionary<(int, int, int), SpotOptions> Map = new();

   public SpottyColor(IColor baseColor, SpotOptions[] spots)
   {
      BaseColor = baseColor;
      Spots = spots;
      foreach (var spot in Spots)
      {
         for (var i = 0; i < spot.CountPer100X100X100; i++)
         {
            var x0 = Random.Shared.Next(-100, 100);
            var y0 = Random.Shared.Next(-100, 100);
            var z0 = Random.Shared.Next(-100, 100);
            
            for (var dx = -(int) spot.Radius; dx <= (int) spot.Radius; dx++)
            for (var dy = -(int) spot.Radius; dy <= (int) spot.Radius; dy++)
            for (var dz = -(int) spot.Radius; dz <= (int) spot.Radius; dz++)
               if (dx * dx + dy * dy + dz * dz <= spot.Radius * spot.Radius)
               {
                  Map[(dx + x0, dy + y0, dz + z0)] = spot;
               }
         }
      }
   }

   public string Render(ComponentDescription cd)
   {
      if (Map.TryGetValue((cd.X, cd.Y, cd.Z), out var spot))
      {
         return spot.Color.Render(cd);
      }

      return BaseColor.Render(cd);
   }
}

public class SpotOptions(float radius, int cnt, IColor color)
{
   public float Radius = radius;
   public int CountPer100X100X100 = cnt;
   public IColor Color = color;
}