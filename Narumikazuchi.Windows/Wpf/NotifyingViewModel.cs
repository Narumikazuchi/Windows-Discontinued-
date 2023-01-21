﻿namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents a view model which implements the <see cref="INotifyPropertyChanged"/> interface.
/// </summary>
public abstract partial class NotifyingViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for this view model.
    /// </summary>
    /// <param name="propertyName">The name of the property that has changed.</param>
    protected void OnPropertyChanged([DisallowNull] String propertyName)
    {
        ArgumentNullException.ThrowIfNull(propertyName);

        this.PropertyChanged?.Invoke(sender: this,
                                     e: new(propertyName));
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;
}