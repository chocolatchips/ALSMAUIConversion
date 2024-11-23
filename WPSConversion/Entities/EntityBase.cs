using System;
using System.ComponentModel;

namespace WPSConversion.Entities
{
    /// <summary>
    /// Representation of an entry in a database table
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityBase<TEntity> : IEntity
        where TEntity : class, IEntity, new()
    {
        /// <summary>Used to ensure unique identifiers for each instance</summary>
        private static int _nextId = 1;

        /// <inheritdoc/>
        public int Id { get; }

        /// <inheritdoc/>
        public bool IsChanged { get; private set; }

        /// <inheritdoc/>
        public bool IsDirty => IsNew || IsChanged;

        /// <inheritdoc/>
        public bool IsNew { get; private set; }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of "Entity" and sets a unique Id value
        /// </summary>
        protected EntityBase()
        {
            Id = _nextId++;
            IsNew = true;
        }

        /// <inheritdoc/>
        public void AcceptChanges()
        {
            IsNew = false;
            IsChanged = false;
            RaisePropertyChanged(nameof(IsDirty));
        }

        /// <summary>
        /// Creates a copy of the current entry. Does not copy the Id.
        /// </summary>
        /// <returns>A copy of the current entry</returns>
        public TEntity Clone()
        {
            TEntity clone = new TEntity();
            clone.MatchProperties(this);
            return clone;
        }

        object ICloneable.Clone() => Clone();

        IEntity IEntity.Clone() => Clone();

        /// <inheritdoc/>
        public void MatchProperties(TEntity fromEntity)
        {
            MatchPropertiesInternal(fromEntity);
        }

        /// <summary>
        /// Copies the property values of the specified entry. Does not copy the Id.
        /// </summary>
        /// <param name="fromEntity">Entry to copy valies from</param>
        protected virtual void MatchPropertiesInternal(TEntity fromEntity) { }

        void IEntity.MatchProperties(IEntity fromEntity) => MatchProperties(fromEntity as TEntity);

        /// <summary>
        /// Sets a value to a field. If it's a new value, raises PropertyChanged event, sets IsChanged to true.
        /// Returs whether the field value changed.
        /// </summary>
        /// <typeparam name="T">Field type</typeparam>
        /// <param name="field">Backing field variable (not the public property)</param>
        /// <param name="value">New value to set</param>
        /// <param name="propertyName">Name of the public property (use nameof)</param>
        /// <returns>Whether the field value changed</returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            bool valueChanged = SetNotifyableProperty(ref field, value, propertyName);

            if (valueChanged)
            {
                IsChanged = true;
                RaisePropertyChanged(nameof(IsDirty));
            }

            return valueChanged;
        }

        /// <summary>
        /// Sets a value to a field. If it's a new value, raises PropertyChanged event. Does not affect IsChanged value.
        /// Returs whether the field value changed.
        /// </summary>
        /// <typeparam name="T">Field type</typeparam>
        /// <param name="field">Backing field variable (not the public property)</param>
        /// <param name="value">New value to set</param>
        /// <param name="propertyName">Name of the public property (use nameof)</param>
        /// <returns>Whether the field value changed</returns>
        protected bool SetNotifyableProperty<T>(ref T field, T value, string propertyName)
        {
            bool valueChanged = !Equals(field, value);

            if (valueChanged)
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }

            return valueChanged;
        }

        /// <inheritdoc/>
        public virtual bool Validate()
        {
            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        /// Raises PropertyChanged for the specified property name
        /// </summary>
        /// <param name="propertyName">Name of the property whos value changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
