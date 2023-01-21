namespace Narumikazuchi.Windows;

public partial struct HslColor : IEquatable<HslColor>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals(HslColor other)
    {
        return this.Hue == other.Hue &&
               this.Saturation == other.Saturation &&
               this.Light == other.Light;
    }

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj)
    {
        return obj is HslColor other &&
               this.Equals(other);
    }

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode()
    {
        return this.Hue.GetHashCode() ^
               this.Saturation.GetHashCode() ^
               this.Light.GetHashCode();
    }

#pragma warning disable
    static public Boolean operator ==(in HslColor left, in HslColor right)
    {
        return left.Equals(right);
    }

    static public Boolean operator !=(in HslColor left, in HslColor right)
    {
        return !left.Equals(right);
    }
#pragma warning restore
}