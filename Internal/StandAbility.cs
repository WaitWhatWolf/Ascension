using System;
using WarWolfWorks_Mod.Interfaces;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Core class for all abilities of a <see cref="Stand"/>.
    /// </summary>
    public abstract class StandAbility : IUpdatable
    {
        /// <summary>
        /// The modplayer who owns this <see cref="StandAbility"/>'s <see cref="Stand"/>.
        /// </summary>
        protected WWWPlayer Owner { get; private set; }

        /// <summary>
        /// Cooldown required for this ability.
        /// </summary>
        public TimeSpan Cooldown { get; protected set; }

        /// <summary>
        /// Current countdown which determines if the cooldown is up.
        /// </summary>
        public TimeSpan Countdown { get; private set; }

        /// <summary>
        /// Sub-type of this ability.
        /// </summary>
        public abstract Type AbilityType { get; }

        /// <summary>
        /// Returns true if <see cref="Countdown"/> is equal to or higher than <see cref="Cooldown"/>.
        /// </summary>
        public bool CooldownUp => Countdown >= Cooldown;

        /// <summary>
        /// Retuns true if this ability is activated.
        /// </summary>
        /// <returns></returns>
        public abstract bool Activates();
        /// <summary>
        /// Called when <see cref="Activates"/> returns true.
        /// </summary>
        public virtual void OnActivate()
        {
            Countdown = TimeSpan.Zero;
        }
        /// <summary>
        /// Called every in-game frame.
        /// </summary>
        public void Update()
        {
            Countdown = Countdown.Add(Utilities.TimespanCounterUF);
            if (Activates()) OnActivate();
            OnUpdate();
        }

        /// <summary>
        /// Called after <see cref="Update"/>, which adds to Countdown.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Initiates this class with an owner.
        /// </summary>
        /// <param name="owner"></param>
        public StandAbility(WWWPlayer owner)
        {
            Owner = owner;
            Countdown = TimeSpan.Zero;
        }
    }
}
