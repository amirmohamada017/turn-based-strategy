using System;

public readonly struct GridPosition : IEquatable<GridPosition>
{
    public bool Equals(GridPosition other)
    {
        return X == other.X && Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

    public int X { get;}
    public int Z { get;}
    
    public GridPosition(int x, int z)
    {
        this.X = x;
        this.Z = z;
    }

    public override string ToString()
    {
        return $"x: {X}, z: {Z}";
    }
    
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.X == b.X && a.Z == b.Z;
    }
    
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X + b.X, a.Z + b.Z);
    }
    
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X - b.X, a.Z - b.Z);
    }
}