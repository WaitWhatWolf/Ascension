using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Projectiles
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SmallPurpleFlame : AscensionProjectile
    {
        bool colided;
        public override void SetDefaults()
        {
            Projectile.width = 16; 
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = -1f;
        }
        public override void AI()
        {
            MakeDust();
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

            public void MakeDust()
            {
                for (int k = 0; k < 1; k++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
                    Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].noGravity = true;
                }

            }
    }
}
