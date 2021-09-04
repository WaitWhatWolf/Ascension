using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources.Trademark;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Thrown by <see cref="Item_ParasiteSlimeJavelin"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/11 18:06:15")]
    public class Projectile_ParasiteSlimeJavelin : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.JavelinFriendly);

            Projectile.width = 60;
            Projectile.height = 20;
            Projectile.penetrate = 1;

            Projectile.DamageType = ModContent.GetInstance<UmbralDamageClass>();
        }

        public override void PostDraw(Color lightColor)
        {
            ASCResources.Dusts.Dust_ParasiteSlime_ProjTravel(Projectile).Create(Projectile.position);
        }

        public override void Kill(int timeLeft)
        {
            ASCResources.Sound.Play_ParasiteSlime_Explosion(Projectile.position);
            Hooks.InGame.ApplyModBuffToAllWithin<Buff_Parasites>(Projectile, Projectile.Center, PARASITESLIME_EXP_RANGE, PARASITESLIME_BUFF_DURATION, ASCResources.Delegates.IsNotSlime);
            ASCResources.Dusts.Dust_ParasiteSlime_Explode.Create(Projectile.position);
        }
    }
}
