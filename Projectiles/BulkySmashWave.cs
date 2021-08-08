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
    class BulkySmashWave : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 2;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 0.75f;
        }
        public override void AI()
        {
            //float rot;
            //rot = Projectile.rotation;
            //Projectile.direction = (int)rot;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.t_Crystal, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item51, Projectile.position);
            target.AddBuff(ModContent.BuffType<Smash>(), 1, true);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item51, Projectile.position);
        }
    }
}
