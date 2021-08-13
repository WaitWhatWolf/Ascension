using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    /// The base class to spawn a wormed slime projectile.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 11), Note(Dev.WaitWhatWolf, "Class is abstract generic to allow for different damage types, see first line in SetDefaults().")]
    public abstract class Projectile_WormedSlimeWorm<T> : AscensionProjectile where T : DamageClass
    {
        public override string Name { get; } = "Projectile_WormedSlimeWorm";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wormed Slime Worm");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<T>();

            Projectile.height = 4;
            Projectile.width = 12;
        }
    }
}
