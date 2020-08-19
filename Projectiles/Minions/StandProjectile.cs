using WarWolfWorks_Mod.Interfaces;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Projectiles.Minions
{
    /// <summary>
    /// In-Game <see cref="Stand"/>; Inherits from <see cref="Terraria.Projectile"/>.
    /// </summary>
    public sealed class StandProjectile : Minion, IUpdatable
    {
        /// <summary>
        /// Called every in-game frame through <see cref="WWWPlayer"/>.
        /// </summary>
        public void Update()
        {
            foreach (StandAbility sa in WWWPlayer.Instance.Stand.Abilities)
            {
                sa.Update();
            }
        }

        /// <summary>
        /// Pointer to <see cref="StandProjectile.Update"/>.
        /// </summary>
        protected override void ActAI() => Update();

        /// <summary>
        /// Returns the owner's <see cref="Stand.Active"/>.
        /// </summary>
        /// <returns></returns>
        protected override bool CheckActive() => WWWPlayer.Instance.Stand.Active;
    }
}
