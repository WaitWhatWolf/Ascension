using System;
using static WarWolfWorks_Mod.Internal.Constants;
using WarWolfWorks_Mod.Buffs;

namespace WarWolfWorks_Mod.Internal
{
    public sealed partial class Stand
    {
        #region CONSTRUCTORS
        /// <summary>
        /// Returns <see href="https://jojo.fandom.com/wiki/Star_Platinum">Star Platinum</see>.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Stand StarPlatinum()
        {
            Stand toReturn = new Stand()
            {
                Name = "Star Platinum",
                Portrait = TEX_UI_STAND_SP_PORTRAIT,
                Abilities = new StandAbility[]
                {
                    new StandMeleeAutoAbility(
                        TimeSpan.FromSeconds(0.1f),
                        ANIM_STARPLATINUM_IDLE,
                        ANIM_STARPLATINUM_ATK)
                },
                ID = StandType.STAR_PLATINUM
            };

            return toReturn;
        }

        /// <summary>
        /// Returns <see href="https://jojo.fandom.com/wiki/The_World">The World</see>.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Stand TheWorld()
        {
            Stand toReturn = new Stand()
            {
                Name = "The World",
                Portrait = TEX_UI_STAND_SP_PORTRAIT,
                Abilities = new StandAbility[]
                {
                    new StandMeleeAutoAbility(TimeSpan.FromSeconds(0.1f), ANIM_THEWORLD_IDLE, ANIM_THEWORLD_ATK)
                },
                ID = StandType.THE_WORLD
            };

            return toReturn;
        }
        #endregion
    }
}
