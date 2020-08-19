using Terraria.ModLoader;
using WarWolfWorks_Mod.Interfaces;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Projectiles.Minions
{
    /// <summary>
    /// Core class for all minions in the WarWolfWorks mod.
    /// </summary>
    public abstract class Minion : ModProjectile, IPostWorldLoadable
    {
        /// <summary>
        /// Overrides the AI by checking if <see cref="CheckActive"/> returns true, then calls <see cref="ActAI"/> and finally calls <see cref="ActTexture"/>'s <see cref="Animation.Update"/>.
        /// </summary>
        public sealed override void AI()
        {
            if (CheckActive())
            {
                ActAI();
                ActTexture.Update();
            }
        }

        /// <summary>
        /// Initiates this <see cref="Minion"/>.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnWorldLoaded()
        {

        }

        /// <summary>
        /// Adds itself to <see cref="WWWPlayer.PostWorldLoadables"/>
        /// </summary>
        public override void SetStaticDefaults()
        {
            WWWPlayer.PostWorldLoadables.Add(this);
        }

        public override bool Autoload(ref string name)
        {
            base.Autoload(ref name);

            return false;
        }

        private Animation ActTexture = Animation.DefaultSafe; //Changed with SetTexture(string to), used by Texture.
        /// <summary>
        /// Currently displayed sprite of this stand.
        /// </summary>
        public override string Texture => ActTexture;

        /// <summary>
        /// Sets the current <see cref="ActTexture"/> to the given animation. (Used by <see cref="Texture"/>)
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
