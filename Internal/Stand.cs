using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using WarWolfWorks_Mod.Buffs;
using WarWolfWorks_Mod.Internal.Debugging.Exceptions;
using WarWolfWorks_Mod.Projectiles.Minions;
using WarWolfWorks_Mod.UI;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Core class of the entire <see cref="Stand"/> system of the WarWolfWorks mod.
    /// </summary>
    public sealed partial class Stand
    {
        /// <summary>
        /// Name of the stand.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Modded Player which owns this stand.
        /// </summary>
        public WWWPlayer Owner { get; private set; }
        /// <summary>
        /// Debuff displayed for this stand.
        /// </summary>
        public StandDebuff Debuff { get; private set; }
        /// <summary>
        /// All abilities of the stand.
        /// </summary>
        public StandAbility[] Abilities { get; private set; }
        /// <summary>
        /// Stand In-Game. (Stand is actually a projectile, blame Terraria, not me)
        /// </summary>
        public StandProjectile Projectile { get; private set; }
        /// <summary>
        /// Stand's ID.
        /// </summary>
        public StandType ID { get; private set; }

        /// <summary>
        /// Determines if the stand is currently drawn/used by the player.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Name of the UI Sprite to be displayed as the passive ability.
        /// </summary>
        public string PassiveAbilityTexture { get; }
        /// <summary>
        /// Name of the UI Sprite to be displayed as the normal ability.
        /// </summary>
        public string NormalAbilityTexture { get; }
        /// <summary>
        /// Name of the UI Sprite to be displayed as the ultimate ability.
        /// </summary>
        public string UltimateAbilityTexture { get; }

        /// <summary>
        /// Returns the appropriate stand debuff based on the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetStandDebuffID(StandType id)
        {
            switch (id)
            {
                default: return -1;
                case StandType.STAR_PLATINUM:
                    return ModContent.BuffType<StarPlatinumDebuff>();
                case StandType.THE_WORLD:
                    return ModContent.BuffType<TheWorldDebuff>();
            }
        }

        /// <summary>
        /// Returns a stand based on the given ID with "to" as it's owner.
        /// </summary>
        /// <example><see cref="StandType.THE_WORLD"/> returns <see cref="Stand.TheWorld(WWWPlayer)"/></example>
        /// <param name="id"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Stand GetStandByID(StandType id, WWWPlayer to)
        {
            switch (id)
            {
                default: return null;
                case StandType.STAR_PLATINUM: return StarPlatinum(to);
                case StandType.THE_WORLD: return TheWorld(to);
            }
        }

        /// <summary>
        /// Should always be called this method when setting a new stand.
        /// </summary>
        public void DefineStand(Player player)
        {
            int toSet = GetStandDebuffID(ID);
            if (toSet != -1)
                player.AddBuff(toSet, int.MaxValue, false);
        }

        /// <summary>
        /// Returns true if the stand is not null.
        /// </summary>
        /// <param name="stand"></param>
        public static implicit operator bool(Stand stand)
            => stand != null;
    }
}
