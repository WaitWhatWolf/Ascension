using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using WarWolfWorks_Mod.Interfaces;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Core class for all abilities of a <see cref="Stand"/>.
    /// </summary>
    public abstract class StandAbility : IUpdatable
    {
        /// <summary>
        /// Cooldown required for this ability.
        /// </summary>
        public TimeSpan Cooldown { get; protected set; }

        /// <summary>
        /// Current countdown which determines if the cooldown is up.
        /// </summary>
        public TimeSpan Countdown { get; protected set; }

        /// <summary>
        /// Sub-type of this ability.
        /// </summary>
        public abstract Type AbilityType { get; }

        /// <summary>
        /// Returns true if <see cref="Countdown"/> is equal to or higher than <see cref="Cooldown"/>.
        /// </summary>
        public bool CooldownUp => Countdown >= Cooldown;

        /// <summary>
        /// The texture used to display the ability on <see cref="StandMenu"/>.
        /// </summary>
        public abstract Texture2D UITexture { get; }

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
            if (Countdown.TotalMilliseconds < 0)
                Countdown = TimeSpan.Zero;
            else if (Countdown.TotalMilliseconds > 0)
                Countdown -= TimeSpan.FromSeconds(1f / Main.frameRate);

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
        public StandAbility()
        {
            Countdown = TimeSpan.Zero;
            WWWPlayer.Updatables.Add(this);
        }

        ~StandAbility()
        {
            WWWPlayer.Updatables.Remove(this);
        }
    }
}
