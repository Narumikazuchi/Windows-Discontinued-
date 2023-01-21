namespace Narumikazuchi.Windows.Wpf;

public partial class Font : IEquatable<Font>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals([AllowNull] Font? other)
    {
        return other is not null &&
               this.Size == other.Size &&
               this.Stretch == other.Stretch &&
               this.Style == other.Style &&
               this.Weight == other.Weight &&
               this.Family.BaseUri == other.Family.BaseUri;
    }

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj)
    {
        return obj is Font other &&
               this.Equals(other);
    }

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode()
    {
        return this.Size.GetHashCode() ^
               this.Stretch.GetHashCode() ^
               this.Style.GetHashCode() ^
               this.Weight.GetHashCode() ^
               this.Family.BaseUri.GetHashCode();
    }

#pragma warning disable
    public static Boolean operator ==(Font? left, Font? right)
    {
        if (left is null)
        {
            return right is null;
        }
        else
        {
            return left.Equals(right);
        }
    }

    public static Boolean operator !=(Font? left, Font? right)
    {
        if (left is null)
        {
            return right is not null;
        }
        else
        {
            return !left.Equals(right);
        }
    }
#pragma warning restore
}