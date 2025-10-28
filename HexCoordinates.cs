using System.Diagnostics.CodeAnalysis;

namespace HexLib;

/// <summary>
/// A struct to store the coordinates of each hexagonal tile within a grid.
/// </summary>
public struct HexCoordinates
{
    public int Q { get; set; }              // column   = \\\\\
    public int R { get; set; }              // row      = -----
    public int Z { get { return -Q - R; } } // columm   = /////

    /// <summary>
    /// An enum containing the directions for a pointy-top hexagon.
    /// </summary>
    public enum HexDirection
    {
        TopRight,
        Right,
        BottomRight,
        BottomLeft,
        Left,
        TopLeft
    }

    private static readonly Dictionary<HexDirection, HexCoordinates> NeighbourOffsets = new()
    {
        { HexDirection.TopRight,    new HexCoordinates( 1, -1) },
        { HexDirection.Right,       new HexCoordinates( 1,  0) },
        { HexDirection.BottomRight, new HexCoordinates( 0,  1) },
        { HexDirection.BottomLeft,  new HexCoordinates(-1,  1) },
        { HexDirection.Left,        new HexCoordinates(-1,  0) },
        { HexDirection.TopLeft,     new HexCoordinates( 0, -1) }
    };

    public HexCoordinates(int q, int r)
    {
        Q = q;
        R = r;
    }

    /// <summary>
    /// Returns the cube coordinates of the current HexCoordinates object.
    /// </summary>
    /// <returns>Cube coordinates converted from the original axial coordinates.</returns>
    public (int q, int r, int z) ToCube() => (Q, R, Z);

    #region Neighbours

    /// <summary>
    /// Returns true if the argument HexCoordinates object is a neighbour of the current.
    /// </summary>
    /// <param name="other">The HexCoordinates to check.</param>
    /// <returns>True if <b>other</b> is a neighbour.</returns>
    public bool IsNeighbour(HexCoordinates other)
    {
        foreach (var offset in NeighbourOffsets)
            if (other.Q == this.Q + offset.Value.Q && other.R == this.R + offset.Value.R)
                return true;
        return false;
    }

    /// <summary>
    /// Yields the cells surrounding the current HexCoordinate. Can be overloaded with a radius argument.
    /// </summary>
    /// <returns>All six cells surrounding the current HexCoordinate.</returns>
    public IEnumerable<HexCoordinates> GetNeighbours()
    {
        foreach (var offset in NeighbourOffsets)
            yield return this + offset.Value;
    }

    /// <summary>
    /// Yields the cells surrounding the current HexCoordinate within a defined radius.
    /// </summary>
    /// <param name="radius">The radius within which cells should be returned.</param>
    /// <returns>All cells within the defined radius of the current HexCoordinate.</returns>
    public IEnumerable<HexCoordinates> GetNeighbours(int radius)
    {
        for (int dq = -radius; dq <= radius; dq++)
            for (int dr = Math.Max(-radius, -dq - radius); dr <= Math.Min(radius, -dq + radius); dr++)
                yield return new HexCoordinates(Q + dq, R + dr);
    }

    public HexCoordinates GetNeighbour(HexDirection direction)
    {
        return this + NeighbourOffsets[direction];
    }

    #endregion

    public static int Distance(HexCoordinates a, HexCoordinates b)
    {
        return (Math.Abs(a.Q - b.Q)
              + Math.Abs(a.R - b.R)
              + Math.Abs(a.Z - b.Z)) / 2;
    }

    public int DistanceTo(HexCoordinates other)
    {
        return (Math.Abs(Q - other.Q)
              + Math.Abs(R - other.R)
              + Math.Abs(Z - other.Z)) / 2;
    }

    public override string ToString() => $"({Q}, {R})";

    public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
        => new(a.Q + b.Q, a.R + b.R);

    public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b)
        => new(a.Q - b.Q, a.R - b.R);

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is HexCoordinates other && Q == other.Q && R == other.R;

    public override int GetHashCode()
        => HashCode.Combine(Q, R);
}