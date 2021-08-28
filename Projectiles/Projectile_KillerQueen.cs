using Ascension.Attributes;
using Ascension.Enums;
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
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            base.SetDefaults();
        }

        public override bool MinionContactDamage() => false;

        public override bool CloneNewInstances => true;

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);
        }
    }
}
