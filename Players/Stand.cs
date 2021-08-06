using Ascension.Enums;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Ascension.ASCResources.Players;
using static Ascension.ASCResources.Stats;

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
        /// The identity of the stand.
        /// </summary>
        public StandID ID { get; }

        /// <summary>
        /// The current level of the stand.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Attempts to upgrade the stand.
        /// </summary>
        /// <returns>True if stand was upgraded.</returns>
        public bool UpgradeStand()
        {
            if(Level == 1 && !Owner.ConsumedRedHotChiliPepper)
            {
                return true;
            }

            return false;
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
        }

        /// <summary>
        /// Opposite of <see cref="Invoke"/>.
        /// </summary>
        public void Recall()
        {
            Main.projectile[pv_InstancedStand].Kill();
            Active = false;
            pv_InstancedStand = -1;
        }

        public int GetStat(string statName)
        {
            return pv_Stats[statName].GetScaledValue(Owner.DefeatedBosses.Count);
        }

        public float GetSingleStat(string statName)
        {
            return pv_Stats[statName].GetSingleScaledValue(Owner.DefeatedBosses.Count);
        }

        public Stand(AscendedPlayer player, StandID id)
        {
            Owner = player;
            Active = false;
            ID = id;
            Name = GetStandName(ID);

            Stat damage = ID switch
            {
                StandID.STAR_PLATINUM => new(5f, 5f, 1),
                _ => default
            };

            Stat armorpen = ID switch
            {
                StandID.STAR_PLATINUM => new(10f, 1.5f, 2),
                _ => default
            };

            Stat attackspeed = ID switch
            {
                StandID.STAR_PLATINUM => new(180f, 20f, 1),
                _ => default
            };

            Stat knockback = ID switch
            {
                StandID.STAR_PLATINUM => new(2f, 1.1f, 2),
                _ => default
            };

            Stat AIrange = ID switch
            {
                StandID.STAR_PLATINUM => new(200f, 5f, 1),
                _ => default
            };

            Stat movespeed = ID switch
            {
                StandID.STAR_PLATINUM => new(20f, 2f, 1),
                _ => default
            };

            pv_Stats.Add(STAND_STAT_DAMAGE, damage);
            pv_Stats.Add(STAND_STAT_ARMORPEN, armorpen);
            pv_Stats.Add(STAND_STAT_ATTACKSPEED, attackspeed);
            pv_Stats.Add(STAND_STAT_KNOCKBACK, knockback);
            pv_Stats.Add(STAND_STAT_MOVESPEED, movespeed);
            pv_Stats.Add(STAND_STAT_AIRANGE, AIrange);
        }

        private Dictionary<string, Stat> pv_Stats = new();
        private int pv_InstancedStand = -1;

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
                    4 => BaseValue / (ScaleFactor * amount),
                    3 => BaseValue - (ScaleFactor * amount),
                    2 => BaseValue * ScaleFactor * amount,
                    _ => BaseValue + (ScaleFactor * amount)
                };
            }

            public int GetScaledValue(int amount)
            {
                int int_BaseValue = (int)BaseValue;
                int int_ScaleFactor = (int)ScaleFactor;

                return ScaleType switch
                {
                    4 => int_BaseValue / (int_ScaleFactor * amount),
                    3 => int_BaseValue - (int_ScaleFactor * amount),
                    2 => int_BaseValue * int_ScaleFactor * amount,
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
