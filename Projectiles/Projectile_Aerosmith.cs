using Ascension.Attributes;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Utility;
using Terraria;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Aerosmith's projectile.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/10/01 11:00:21")]
    public class Projectile_Aerosmith : Projectile_Stand_WAura<Dust_Stand_Aerosmith_Aura>
    {
        public override Animator StandAnimator { get; } = new(ASCResources.Animations.Stand_Aerosmith);

        protected override bool StandUserEmitsAura { get; } = true;

        protected override int AfterImageLength { get; } = 12;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;

            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 138;
            Projectile.DamageType = pr_Stand.Class;

            base.SetDefaults();
        }
    }
}
