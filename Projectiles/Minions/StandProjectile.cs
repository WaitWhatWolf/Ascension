using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Projectiles.Minions
{
    /// <summary>
    /// In-Game <see cref="Stand"/>; Inherits from <see cref="Terraria.Projectile"/>.
    /// </summary>
    public sealed class StandProjectile : Minion
    {
        /// <summary>
        /// Calls <see cref="StandAbility.OnUpdate"/> every frame, while checking <see cref="StandAbility.Activates"/> for <see cref="StandAbility.OnActivate"/>.
        /// </summary>
        protected override void ActAI()
        {
            if (!CheckActive())
                return;

            foreach(StandAbility sa in WWWOwner.Stand.Abilities)
            {
                sa.OnUpdate();
                if (sa.Activates())
                    sa.OnActivate();
            }
        }

        /// <summary>
        /// Returns the owner's <see cref="Stand.Active"/>.
        /// </summary>
        /// <returns></returns>
        protected override bool CheckActive() => WWWOwner.Stand.Active;
    }
}
