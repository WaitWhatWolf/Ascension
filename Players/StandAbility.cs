using Ascension.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Players
{
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
        public virtual string TooltipText => Name + '\n' + Description;

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
            if (!CooldownReady)
            {
                pr_CurrentCooldown -= ASCResources.FLOAT_PER_FRAME;
                if(pr_CurrentCooldown <= 0)
                {
                    CooldownReady = true;
                }
            }
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
        public float GetCooldown() => Cooldown;

        /// <summary>
        /// Returns the current cooldown of this ability.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentCooldown() => pr_CurrentCooldown;

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
        /// When invoked, the cooldown timer is reset and <see cref="CooldownReady"/> is set to false.
        /// </summary>
        protected void ResetCooldown()
        {
            pr_CurrentCooldown = Cooldown;
            CooldownReady = false;
        }

        /// <summary>
        /// The cooldown of this ability.
        /// </summary>
        protected abstract float Cooldown { get; }

        /// <summary>
        /// Returns true if the cooldown is off; Handled in base <see cref="Update"/>.
        /// </summary>
        protected bool CooldownReady { get; set; }

        /// <summary>
        /// The current cooldown timer of this ability.
        /// </summary>
        protected float pr_CurrentCooldown;
    }
}
