using Terraria.ModLoader;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Projectiles.Minions
{
    /// <summary>
    /// Core class for all minions in the WarWolfWorks mod.
    /// </summary>
    public abstract class Minion : ModProjectile
    {
        /// <summary>
        /// Overrides the AI by checking if <see cref="CheckActive"/> returns true, then calls <see cref="ActAI"/> and finally calls <see cref="Animate"/>.
        /// </summary>
        public sealed override void AI()
        {
            if (CheckActive())
            {
                ActAI();
            }
        }

        /// <summary>
        /// Player owner of this <see cref="Minion"/>.
        /// </summary>
        protected Terraria.Player Owner;
        /// <summary>
        /// WWWPlayer owner of this <see cref="Minion"/>.
        /// </summary>
        protected WWWPlayer WWWOwner;

        /// <summary>
        /// Sets the defaults of this <see cref="Minion"/>.
        /// </summary>
        public override void SetDefaults()
        {
            //Attempts to set the non-modded owner.
            Owner = projectile.owner != -1 ? Terraria.Main.player[projectile.owner] : projectile.owner == 255 ? Terraria.Main.LocalPlayer : null;
            //If non-modded owner was successfully set, modded owner will be set as well.
            if (Owner != null) WWWOwner = Owner.GetModPlayer<WWWPlayer>();
        }

        private Animation ActTexture; //Changed with SetTexture(string to), used by Texture.
        /// <summary>
        /// Currently displayed sprite of this stand.
        /// </summary>
        public override string Texture => ActTexture;

        /// <summary>
        /// Sets the current 
        /// </summary>
        /// <param name="to"></param>
        public void SetAnimation(Animation to)
        {
            ActTexture = new Animation(to);
            ActTexture.Start();
        }

        /// <summary>
        /// Returns true when this <see cref="Minion"/> is active in the game.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckActive();
        /// <summary>
        /// Actual AI code.
        /// </summary>
        protected abstract void ActAI();
    }
}
