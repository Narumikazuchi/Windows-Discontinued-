namespace Narumikazuchi.Windows;

public partial struct HsvColor : IEquatable<HsvColor>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals(HsvColor other)
    {
        return this.Hue == other.Hue &&
               this.Saturation == other.Saturation &&
               this.Value == other.Value;
    }

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj)
    {
        return obj is HsvColor other &&
               this.Equals(other);
    }

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode()
    {
        return this.Hue.GetHashCode() ^
               this.Saturation.GetHashCode() ^
               this.Value.GetHashCode();
    }

#pragma warning disable
    static public Boolean operator ==(in HsvColor left, in HsvColor right)
    {
        return left.Equals(right);
    }

    static public Boolean operator !=(in HsvColor left, in HsvColor right)
    {
        return !left.Equals(right);
    }
#pragma warning restore
}