using Ascension.Attributes;
using Ascension.Dusts;
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
    class FallingBlades : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14; //just like the sword(change image then)
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 900;
        }
        public override void AI()
        {
            Projectile.MaxUpdates = 1;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Sparkle>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) Projectile.tileCollide = true;
            if (Projectile.tileCollide)
            {
                //int dust = Dust.NewDust(new Vector2((float)Projectile.position.X, (float)Projectile.position.Y), Projectile.width, Projectile.height, 57, 0, 0, 100, Color.White, 1f);
                //Main.dust[dust].noGravity = false;
            }
            Projectile.rotation += 0.3f;
        }
        /*
        
        public override void AI()
        {
            Projectile.velocity.Y += Projectile.ai[0];
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Sparkle>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
        */

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                //Main Projectile AI
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity *= 0.75f;
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Sparkle>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item39, Projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0] += 0.1f;
            Projectile.velocity *= 0.75f;
        }
        
    }
}
