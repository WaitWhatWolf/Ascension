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
    class DemonPortalWave : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 25;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 1;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = (float)Projectile.direction;
            MakeDust();
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item40, Projectile.position);
        }

        public void MakeDust()
        {
                int dust = Dust.NewDust(Projectile.position, Projectile.width + Main.rand.Next(-50, 51), Projectile.height + Main.rand.Next(-50, 51), DustID.PurpleTorch, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;
        }
    }
}
