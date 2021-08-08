using Ascension.Enums;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Ascension.ASCResources.Players;
using static Ascension.ASCResources.Stats;
using static Ascension.ASCResources.Sound;
using Microsoft.Xna.Framework.Graphics;
using Ascension.UI;
using Terraria.UI;
using ReLogic.Content;

namespace Ascension.Players
{
    /// <summary>
    /// Core class for Stands.
    /// </summary>
    public sealed class Stand
    {
        /// <summary>
        /// The player who owns this stand.
        /// </summary>
        public AscendedPlayer Owner { get; }
        /// <summary>
        /// Is the stand currently fighting for the player?
        /// </summary>
        public bool Active { get; private set; }
        /// <summary>
        /// The name of the stand.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The tooltip of the stand.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The identity of the stand.
        /// </summary>
        public StandID ID { get; }

        /// <summary>
        /// The current level of the stand.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Menu used by the stand.
        /// </summary>
        public Menu_Stand StandMenu { get; private set; }

        /// <summary>
        /// The user interface used with <see cref="StandMenu"/>.
        /// </summary>
        public UserInterface StandMenuUI { get; private set; }

        /// <summary>
        /// The portrait texture of this stand.
        /// </summary>
        public Asset<Texture2D> Portrait { get; private set; }

        /// <summary>
        /// All abilities of this stand.
        /// </summary>
        public StandAbility[] Abilities { get; private set; }

        /// <summary>
        /// Returns the mod stand projectile of this stand.
        /// </summary>
        /// <returns></returns>
        public StandProjectile GetStandModProjectile() => (StandProjectile)Main.projectile[pv_InstancedStand].ModProjectile;

        /// <summary>
        /// Returns the normal stand projectile of this stand.
        /// </summary>
        /// <returns></returns>
        public Projectile GetStandProjectile() => Main.projectile[pv_InstancedStand];

        /// <summary>
        /// Returns the current movement AI used by the stand.
        /// </summary>
        /// <returns></returns>
        public Action GetCurrentMovementAI() => pv_MovementAIs.Count > 0 ? pv_MovementAIs[0] : pv_BaseMovementAI;

        /// <summary>
        /// Adds a movement AI; The most recent AI will be used instead of the base movement AI.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool AddMovementAI(Action action)
        {
            if (pv_MovementAIs.Contains(action))
                return false;

            pv_MovementAIs.Insert(0, action);
            return true;
        }
        
        /// <summary>
        /// Removes a given MovementAI; If no more custom movement AI's are present, the movement AI will return to the base movement AI.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RemoveMovementAI(Action action)
        {
            return pv_MovementAIs.Remove(action);
        }

        /// <summary>
        /// Sets the base movement AI.
        /// </summary>
        /// <param name="AI"></param>
        public void SetBaseMovementAI(Action AI)
        {
            pv_BaseMovementAI = AI;
        }

        /// <summary>
        /// Attempts to upgrade the stand.
        /// </summary>
        /// <returns>True if stand was upgraded.</returns>
        public int UpgradeStand(bool debugUpgrade = true)
        {
            int toReturn = 0;
            if(Level == 1 && Owner.ConsumedRedHotChiliPepper)
            {
                if(debugUpgrade)
                {
                    Debug.Log($"{Name} has awakened the {Abilities[1].Name} ability!");
                }
                Level++;
                toReturn++;
            }

            return toReturn;
        }

        /// <summary>
        /// Calls the stand to fight for the player.
        /// </summary>
        public void Invoke()
        {
            Active = true;

            switch (ID)
            {
                case StandID.STAR_PLATINUM:
                    pv_InstancedStand = Projectile.NewProjectile(new ProjectileSource_Stand(Owner, this), Owner.Player.Center, 
                        Vector2.Zero, ModContent.ProjectileType<StarPlatinum>(), GetStat(STAND_STAT_DAMAGE), 
                        GetSingleStat(STAND_STAT_KNOCKBACK));
                    ((StarPlatinum)Main.projectile[pv_InstancedStand].ModProjectile).SetupStand(Owner.Player, this);
                    break;
                default: Active = false; break;
            }

            SoundEngine.PlaySound(pv_InstancedStand, Owner.Player.Center);
            StandMenu.ActivateMenu();
        }

        /// <summary>
        /// Opposite of <see cref="Invoke"/>.
        /// </summary>
        public void Recall()
        {
            Main.projectile[pv_InstancedStand].Kill();
            Active = false;
            pv_InstancedStand = -1;
            StandMenu.DeactivateMenu();
        }

        /// <summary>
        /// This should be called by the player every frame.
        /// </summary>
        public void Update()
        {
            for(int i = 0; i < Level; i++)
            {
                Abilities[i].Update();
                //This makes it so even if the stand is recalled, it will still cool down all abilities.
                if (Active)
                {
                    //Tries to activate this ability if it's either the basic attack or the correct key was pressed.
                    if (i == 0 || ASCResources.Input.GetStandAbilityKey(i).JustPressed)
                        Abilities[i].TryActivate();

                    Abilities[i].TryDeactivate();
                }
            }
        }

        public int GetStat(string statName)
        {
            int toUse = Owner.DefeatedBosses.Count;
            if (Level > 1) toUse += 5;
            return pv_Stats[statName].GetScaledValue(toUse);
        }

        public float GetSingleStat(string statName)
        {
            int toUse = Owner.DefeatedBosses.Count;
            if (Level > 1) toUse += 5;
            return pv_Stats[statName].GetSingleScaledValue(toUse);
        }

        public Stand(AscendedPlayer player, StandID id)
        {
            Owner = player;
            Active = false;
            ID = id;
            Name = GetStandName(ID);
            Level = 1;

            Stat damage = default;
            Stat attackrange = default;
            Stat armorpen = default;
            Stat attackspeed = default;
            Stat knockback = default;
            Stat AIrange = default;
            Stat movespeed = default;

            switch (ID) //Add stats here
            {
                case StandID.STAR_PLATINUM:
                    damage = new(5f, 2.5f, 1);
                    attackrange = new Stat(40f, 5f, 1);
                    armorpen = new(10f, 3f, 1);
                    attackspeed = new(180f, 20f, 1);
                    knockback = new(2f, 0.1f, 2);
                    AIrange = new(200f, 5f, 1);
                    movespeed = new(20f, 0.1f, 2);

                    pv_InvokeSoundIndex = ASCResources.Sound.Stand_StarPlatinum_Invoke_Index;
                    Portrait = ASCResources.Textures.Stand_Portrait_StarPlatinum;
                    this.Description = "Star Platinum, a stand which excels at everything outside of range.";
                    break;
            }

            pv_Stats.Add(STAND_STAT_DAMAGE, damage);
            pv_Stats.Add(STAND_STAT_ATTACKRANGE, attackrange);
            pv_Stats.Add(STAND_STAT_ARMORPEN, armorpen);
            pv_Stats.Add(STAND_STAT_ATTACKSPEED, attackspeed);
            pv_Stats.Add(STAND_STAT_KNOCKBACK, knockback);
            pv_Stats.Add(STAND_STAT_MOVESPEED, movespeed);
            pv_Stats.Add(STAND_STAT_AIRANGE, AIrange);

            switch (ID) //Add abilities here
            {
                case StandID.STAR_PLATINUM:
                    Abilities = new StandAbility[]
                    {
                        new StandAbility_StarPlatinum_Punch(this),
                        new StandAbility_StarPlatinum_ORA(this),
                    };
                    break;
            }

            if (pv_BaseMovementAI == null)
                throw new Exception("The stand couldn't be created as there was no ability which set the base movement AI.");

            StandMenuUI = new UserInterface();
            StandMenu = new Menu_Stand(this);
            StandMenuUI.SetState(StandMenu);
        }

        private Action pv_BaseMovementAI;
        private Dictionary<string, Stat> pv_Stats = new();
        private List<Action> pv_MovementAIs = new();
        private int pv_InstancedStand = -1;
        private int pv_InvokeSoundIndex = -1;

        public static implicit operator bool(Stand stand) => stand != null && stand.ID != StandID.NEWBIE;

        private struct Stat
        {
            public float BaseValue;
            public float ScaleFactor;
            public int ScaleType;

            public float GetSingleScaledValue(int amount)
            {
                return ScaleType switch
                {
                    4 => BaseValue / (ScaleFactor * (amount + 1)),
                    3 => BaseValue - (ScaleFactor * amount),
                    2 => BaseValue * (1f + (ScaleFactor * (amount + 1))),
                    _ => BaseValue + (ScaleFactor * amount)
                };
            }

            public int GetScaledValue(int amount)
            {
                int int_BaseValue = (int)BaseValue;
                int int_ScaleFactor = (int)ScaleFactor;

                return ScaleType switch
                {
                    4 => int_BaseValue / (int_ScaleFactor * (amount + 1)),
                    3 => int_BaseValue - (int_ScaleFactor * amount),
                    2 => int_BaseValue * int_ScaleFactor * (amount + 1),
                    _ => int_BaseValue + (int_ScaleFactor * amount)
                };
            }

            /// <summary>
            /// Creates a new stat.
            /// </summary>
            /// <param name="baseVal">The base value of the stat.</param>
            /// <param name="scaleFactor">By how much the stat scales per boss defeated.</param>
            /// <param name="scaleType">
            /// The way ScaleFactor is applied to <see cref="BaseValue"/>:
            /// <list type="number">
            /// <item>Additive</item>
            /// <item>Multiplicative</item>
            /// <item>Substractive</item>
            /// <item>Divisive</item>
            /// </list>
            /// </param>
            public Stat(float baseVal, float scaleFactor, int scaleType)
            {
                BaseValue = baseVal;
                ScaleFactor = scaleFactor;
                ScaleType = scaleType;
            }
        }
    }
}
