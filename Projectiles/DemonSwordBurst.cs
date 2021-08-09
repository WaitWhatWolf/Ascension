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
    class DemonSwordBurst : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 148;
            Projectile.height = 138;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Main.projFrames[Projectile.type] = 3;
            Projectile.penetrate = 50;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 1.25f;
        }
        public override void AI()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }

            if (Projectile.frame > 2)
            {
                Projectile.frame = 2;
            }
            MakeDust();

        }
        
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item42, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        public void MakeDust()
        {
            for (int k = 0; k < 3; k++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width + Main.rand.Next(-50, 51), Projectile.height + Main.rand.Next(-50, 51), DustID.PinkTorch, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
