﻿using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public abstract class StandAbility : IStandReferencable
    {
        /// <summary>
        /// Reference to the stand this ability belongs to.
        /// </summary>
        public Stand Stand { get; }
        
        /// <summary>
        /// Name of this ability.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// The description of this ability.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The text used for tooltips.
        /// </summary>
        public virtual string TooltipText => Hooks.Colors.GetColoredTooltipText(Name, Hooks.Colors.Tangelo) + '\n' + Description;

        /// <summary>
        /// The icon of this ability to be displayed in the UI.
        /// </summary>
        public abstract Asset<Texture2D> Icon { get; }

        /// <summary>
        /// The active state of this ability.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Called every frame; Handles the cooldown, make sure to include "base.Update();" if you want it to cooldown normally.
        /// </summary>
        public virtual void Update() 
        {
            if (!CountdownReady && Countdown.CooldownDone())
                CountdownReady = true;
        }

        /// <summary>
        /// Attempts to activate this ability.
        /// </summary>
        /// <returns>True if the ability is successfully activated.</returns>
        public bool TryActivate()
        {
            if (Active)
                return false;

            if (!ActivateCondition())
                return false;

            Active = true;
            OnActivate();

            return true;
        }

        /// <summary>
        /// Attempts to deactivate this ability.
        /// </summary>
        /// <returns>True if the ability is successfully deactivated.</returns>
        public bool TryDeactivate()
        {
            if (!Active)
                return false;

            if (!DeactivateCondition())
                return false;

            Active = false;
            OnDeactivate();

            return true;
        }

        /// <summary>
        /// Base constructor for all stand abilities.
        /// </summary>
        /// <param name="stand"></param>
        public StandAbility(Stand stand)
        {
            Stand = stand;
            stand.Owner.OnNewBossDefeated += Event_OnNewBossDefeated;
            Event_OnNewBossDefeated(string.Empty);
        }

        /// <summary>
        /// Returns the cooldown of this ability.
        /// </summary>
        /// <returns></returns>
        public float GetCooldown(int truncate = 2) => Countdown.Countdown.Truncate(truncate);

        /// <summary>
        /// Returns the current cooldown of this ability.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentCountdown() => Countdown.GetCurrentCountdown();

        /// <summary>
        /// Invoked by <see cref="AscendedPlayer.OnNewBossDefeated"/> and when the ability is created.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void Event_OnNewBossDefeated(string obj) { }

        /// <summary>
        /// Invoked when the ability is activated.
        /// </summary>
        protected abstract void OnActivate();
        /// <summary>
        /// Invoked when the ability is deactivated.
        /// </summary>
        protected abstract void OnDeactivate();

        /// <summary>
        /// If true, this ability will activate.
        /// </summary>
        /// <returns></returns>
        protected abstract bool ActivateCondition();
        /// <summary>
        /// If true, this ability will deactivate.
        /// </summary>
        /// <returns></returns>
        protected abstract bool DeactivateCondition();

        /// <summary>
        /// When invoked, the cooldown timer is reset and <see cref="CountdownReady"/> is set to false.
        /// </summary>
        protected void ResetCooldown()
        {
            CountdownReady = false;
            Countdown.Reset();
        }

        /// <summary>
        /// The cooldown of this ability.
        /// </summary>
        protected abstract ReturnCountdown Countdown { get; }

        /// <summary>
        /// Returns true if the cooldown is off; Handled in base <see cref="Update"/>.
        /// </summary>
        protected bool CountdownReady { get; set; }
    }
}
