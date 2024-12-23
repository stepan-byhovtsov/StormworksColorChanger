namespace SwColorChanger.Colors;

public class RandFromSetColor : IColor
{
   // 0.3, 0.3, 0.4
   // ind = 0.4
   // 
   public (IColor color, float probability)[] ColorSet;

   public RandFromSetColor((IColor color, float probability)[] colorSet)
   {
      var sum = colorSet.Aggregate(0f, (sum, color) => sum + color.probability);
      var cnt = colorSet.Aggregate(0, (cnt, color) => cnt + color.probability > 0f ? 1 : 0);
      var avg = (1f - sum) / cnt;
      colorSet = colorSet.Select(c => (c.color, c.probability > 0 ? c.probability : avg)).ToArray();
      ColorSet = colorSet;
   }
   
   public string Render()
   {
      var ind = Random.Shared.NextSingle();
      var cur = 0f;
      for (int i = 0; i < ColorSet.Length; i++)
      {
         cur += ColorSet[i].probability;
         if (cur > ind)
         {
            return ColorSet[i].color.Render();
         }
      }

      throw new InvalidOperationException("Probabilities sum was greater than 1");
   }
}