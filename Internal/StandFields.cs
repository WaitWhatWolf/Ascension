using System;
using System.Collections.Generic;
using WarWolfWorks_Mod.Buffs;

namespace WarWolfWorks_Mod.Internal
{
    public sealed partial class Stand
    {
        #region CONST & READONLY
        private const string ANIMBASENAME_STARPLATINUM_IDLE = "SPIDLEA_",
            ANIMBASENAME_STARPLATINUM_ATK = "SPATKA_";

        private static readonly Animation ANIM_STARPLATINUM_IDLE = new Animation
            (
            new KeyValuePair<TimeSpan, string>(new TimeSpan(0), ANIMBASENAME_STARPLATINUM_IDLE + "0"),
            new KeyValuePair<TimeSpan, string>(TimeSpan.FromSeconds(0.05d), ANIMBASENAME_STARPLATINUM_IDLE + "1")
            );

        private static readonly Animation ANIM_STARPLATINUM_ATK = new Animation
            (
            new KeyValuePair<TimeSpan, string>(new TimeSpan(0), ANIMBASENAME_STARPLATINUM_ATK + "0"),
            new KeyValuePair<TimeSpan, string>(TimeSpan.FromSeconds(0.05d), ANIMBASENAME_STARPLATINUM_ATK + "1")
            );

        private const string ANIMBASENAME_THEWORLD_IDLE = "TWIDLEA_",
            ANIMBASENAME_THEWORLD_ATK = "TWATKA_";

        private static readonly Animation ANIM_THEWORLD_IDLE = new Animation
           (
           new KeyValuePair<TimeSpan, string>(new TimeSpan(0), ANIMBASENAME_THEWORLD_IDLE + "0"),
           new KeyValuePair<TimeSpan, string>(TimeSpan.FromSeconds(0.05d), ANIMBASENAME_THEWORLD_IDLE + "1")
           );

        private static readonly Animation ANIM_THEWORLD_ATK = new Animation
            (
            new KeyValuePair<TimeSpan, string>(new TimeSpan(0), ANIMBASENAME_THEWORLD_ATK + "0"),
            new KeyValuePair<TimeSpan, string>(TimeSpan.FromSeconds(0.05d), ANIMBASENAME_THEWORLD_ATK + "1")
            );
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Returns <see href="https://jojo.fandom.com/wiki/Star_Platinum">Star Platinum</see>.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Stand StarPlatinum(WWWPlayer owner)
        {
            Stand toReturn = new Stand()
            {
                Name = "Star Platinum",
                Debuff = new StarPlatinumDebuff(),
                Abilities = new StandAbility[]
                {
                    new StandMeleeAutoAbility(
                        owner,
                        TimeSpan.FromSeconds(0.1f),
                        ANIM_STARPLATINUM_IDLE,
                        ANIM_STARPLATINUM_ATK)
                },
                ID = StandType.STAR_PLATINUM
            };

            toReturn.SetOwner(owner);

            return toReturn;
        }

        /// <summary>
        /// Returns <see href="https://jojo.fandom.com/wiki/The_World">The World</see>.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Stand TheWorld(WWWPlayer owner)
        {
            Stand toReturn = new Stand()
            {
                Name = "The World",
                Debuff = new TheWorldDebuff(),
                Abilities = new StandAbility[]
                {
                    new StandMeleeAutoAbility(owner, TimeSpan.FromSeconds(0.1f), ANIM_THEWORLD_IDLE, ANIM_THEWORLD_ATK)
                },
                ID = StandType.THE_WORLD
            };

            toReturn.SetOwner(owner);

            return toReturn;
        }
        #endregion
    }
}
