namespace HexLib;

/// <summary>
/// A purely static class containing methods to generate different shapes of grid.
/// </summary>
public static class HexGrid
{
    public static IEnumerable<HexCoordinates> GenerateHexagon(int radius)
    {
        for (int dq = -radius; dq <= radius; dq++)
            for (int dr = Math.Max(-radius, -dq - radius); dr <= Math.Min(radius, -dq + radius); dr++)
                yield return new HexCoordinates(dq, dr);
    }

    public static IEnumerable<HexCoordinates> GenerateRectangle(int width, int height)
    {
        for (int q = 0; q <= width; q++)
            for (int r = 0; r <= height; r++)
                yield return new HexCoordinates(q, r);
    }
    
    public static IEnumerable<HexCoordinates> GenerateTriangle(int size)
    {
        for (int q=0; q <= size; q++)
            for (int r=0; r <= size - q; r++)
                yield return new HexCoordinates(q, r);
    }
}