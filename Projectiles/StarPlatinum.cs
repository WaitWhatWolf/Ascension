using Ascension.Attributes;
using Ascension.Enums;
using Terraria;

namespace Ascension.Projectiles
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class StarPlatinum : StandProjectile
    {
        /// <inheritdoc/> //Not sure what to put as desc here lol
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.DamageType = ASCResources.Stats.DamageClass_Umbral;
            base.SetDefaults();
        }

        public override bool CloneNewInstances => true;

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);
        }
    }
}
