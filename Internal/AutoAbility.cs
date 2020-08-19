using System;
namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Automatic ability of a stand, which usually is the starting ability of a stand.
    /// </summary>
    public abstract class AutoAbility : StandAbility
    {
        /// <summary>
        /// Initiates this class with the <see cref="StandAbility"/> constructor.
        /// </summary>
        /// <param name="owner"></param>
        public AutoAbility() : base()
        {
        }

        /// <summary>
        /// This ability's sub-type, sealed to <see cref="AutoAbility"/> type.
        /// </summary>
        public sealed override Type AbilityType => typeof(AutoAbility);
    }
}
