using System.ComponentModel;
using System;

namespace WPFConversion.Entities
{
    public interface IEntity : INotifyPropertyChanged, ICloneable
    {
        /// <summary>Unique identifier</summary>
        int Id { get; }

        /// <summary>Whether field values have changed since last save</summary>
        bool IsChanged { get; }

        /// <summary>Whether the entry is new or has unsaved changes</summary>
        bool IsDirty { get; }

        /// <summary>Whether the entry is new (has not been saved yet)</summary>
        bool IsNew { get; }

        /// <summary>
        /// Accepts changes to the entry (sets IsNew and IsChanged to false)
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Creates a copy of the current entry. Does not copy the Id.
        /// </summary>
        /// <returns>A copy of the current entry</returns>
        new IEntity Clone();

        /// <summary>
        /// Copies the property values of the specified entry. Does not copy the Id.
        /// </summary>
        /// <param name="fromEntity">Entry to copy valies from</param>
        void MatchProperties(IEntity fromEntity);

        /// <summary>
        /// Whether all of the entry's property values are valid
        /// </summary>
        /// <returns>Property valies validity</returns>
        bool Validate();
    }
}