using System.Drawing;

namespace SwColorChanger;

public class Perlin3d
{
    private readonly byte[] _permutationTable;

    public Perlin3d(int seed = 0)
    {
        var rand = new Random(seed);
        _permutationTable = new byte[1024];
        rand.NextBytes(_permutationTable);
    }

    private float[] GetPseudoRandomGradientVector(int x, int y, int z)
    {
        int v = (int)(((x * 1836311903) ^ (y * 2971215073) ^ (z * 4807526976)) & 1023);
        v = _permutationTable[v] & 15;

        switch (v)
        {
            case 0: return new float[] { 1, 1, 0 };
            case 1: return new float[] { -1, 1, 0 };
            case 2: return new float[] { 1, -1, 0 };
            case 3: return new float[] { -1, -1, 0 };
            case 4: return new float[] { 1, 0, 1 };
            case 5: return new float[] { -1, 0, 1 };
            case 6: return new float[] { 1, 0, -1 };
            case 7: return new float[] { -1, 0, -1 };
            case 8: return new float[] { 0, 1, 1 };
            case 9: return new float[] { 0, -1, 1 };
            case 10: return new float[] { 0, 1, -1 };
            case 11: return new float[] { 0, -1, -1 };
            case 12: return new float[] { 1, 1, 0 };
            case 13: return new float[] { -1, 1, 0 };
            case 14: return new float[] { 0, -1, 1 };
            default: return new float[] { 0, -1, -1 };
        }
    }

    public static Color Lerp(Color a, Color b, float t)
    {
        return Color.FromArgb((int)Lerp(a.R, b.R, t), (int)Lerp(a.G, b.G, t), (int)Lerp(a.B, b.B, t));
    }

    public static float QuinticCurve(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    static float Dot(float[] a, float[] b)
    {
        return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
    }

    public float Noise(float fx, float fy, float fz)
    {
        int left = (int)System.Math.Floor(fx);
        int top = (int)System.Math.Floor(fy);
        int front = (int)System.Math.Floor(fz);

        float pointInCubeX = fx - left;
        float pointInCubeY = fy - top;
        float pointInCubeZ = fz - front;

        float[] topLeftFrontGradient = GetPseudoRandomGradientVector(left, top, front);
        float[] topRightFrontGradient = GetPseudoRandomGradientVector(left + 1, top, front);
        float[] bottomLeftFrontGradient = GetPseudoRandomGradientVector(left, top + 1, front);
        float[] bottomRightFrontGradient = GetPseudoRandomGradientVector(left + 1, top + 1, front);

        float[] topLeftBackGradient = GetPseudoRandomGradientVector(left, top, front + 1);
        float[] topRightBackGradient = GetPseudoRandomGradientVector(left + 1, top, front + 1);
        float[] bottomLeftBackGradient = GetPseudoRandomGradientVector(left, top + 1, front + 1);
        float[] bottomRightBackGradient = GetPseudoRandomGradientVector(left + 1, top + 1, front + 1);

        float[] distanceToTopLeftFront = new float[] { pointInCubeX, pointInCubeY, pointInCubeZ };
        float[] distanceToTopRightFront = new float[] { pointInCubeX - 1, pointInCubeY, pointInCubeZ };
        float[] distanceToBottomLeftFront = new float[] { pointInCubeX, pointInCubeY - 1, pointInCubeZ };
        float[] distanceToBottomRightFront = new float[] { pointInCubeX - 1, pointInCubeY - 1, pointInCubeZ };

        float[] distanceToTopLeftBack = new float[] { pointInCubeX, pointInCubeY, pointInCubeZ - 1 };
        float[] distanceToTopRightBack = new float[] { pointInCubeX - 1, pointInCubeY, pointInCubeZ - 1 };
        float[] distanceToBottomLeftBack = new float[] { pointInCubeX, pointInCubeY - 1, pointInCubeZ - 1 };
        float[] distanceToBottomRightBack = new float[] { pointInCubeX - 1, pointInCubeY - 1, pointInCubeZ - 1 };

        float tx1 = Dot(distanceToTopLeftFront, topLeftFrontGradient);
        float tx2 = Dot(distanceToTopRightFront, topRightFrontGradient);
        float bx1 = Dot(distanceToBottomLeftFront, bottomLeftFrontGradient);
        float bx2 = Dot(distanceToBottomRightFront, bottomRightFrontGradient);

        float tx3 = Dot(distanceToTopLeftBack, topLeftBackGradient);
        float tx4 = Dot(distanceToTopRightBack, topRightBackGradient);
        float bx3 = Dot(distanceToBottomLeftBack, bottomLeftBackGradient);
        float bx4 = Dot(distanceToBottomRightBack, bottomRightBackGradient);

        pointInCubeX = QuinticCurve(pointInCubeX);
        pointInCubeY = QuinticCurve(pointInCubeY);
        pointInCubeZ = QuinticCurve(pointInCubeZ);

        float tx = Lerp(tx1, tx2, pointInCubeX);
        float bx = Lerp(bx1, bx2, pointInCubeX);
        float tb = Lerp(tx, bx, pointInCubeY);

        float txb = Lerp(tx3, tx4, pointInCubeX);
        float bxb = Lerp(bx3, bx4, pointInCubeX);
        float tbb = Lerp(txb, bxb, pointInCubeY);

        return Lerp(tb, tbb, pointInCubeZ);
    }

    public float Noise(float fx, float fy, float fz, int octaves, float persistence = 0.5f)
    {
        float amplitude = 1;
        float max = 0;
        float result = 0;

        while (octaves-- > 0)
        {
            max += amplitude;
            result += Noise(fx, fy, fz) * amplitude;
            amplitude *= persistence;
            fx *= 2;
            fy *= 2;
            fz *= 2;
        }

        return result / max;
    }
}