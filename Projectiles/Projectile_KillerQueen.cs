using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Terraria;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Stand projectile for Killer Queen.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class Projectile_KillerQueen : StandProjectile
    {
        /// <inheritdoc/> //Not sure what to put as desc here lol
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
           
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            base.SetDefaults();
        }

        public override Animator StandAnimator { get; } = new(ASCResources.Animations.Stand_KillerQueen);

        public override bool MinionContactDamage() => false;

        public override bool CloneNewInstances => true;

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);
        }
    }
}
