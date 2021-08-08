using Ascension.Buffs;
using Ascension.Dusts;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Projectiles
{
    class PlayerSmashWave : AscensionProjectile
    {
        int counter;
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 2;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 1;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            counter++;
            //float rot;
            //rot = Projectile.rotation;
            //Projectile.direction = (int)rot;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Iron, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            if (counter > 20)
            {
                Projectile.tileCollide = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item51, Projectile.position);
            target.AddBuff(ModContent.BuffType<FriendlySmash>(), 1, true);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item51, Projectile.position);
        }
    }
}
