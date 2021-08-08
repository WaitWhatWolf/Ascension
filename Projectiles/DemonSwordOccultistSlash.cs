using Ascension.Dusts;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Projectiles
{
    class DemonSwordOccultistSlash : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 80;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 1;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 0.75f;
        }
        public override void AI()
        {
            float rot;
            rot = Projectile.rotation;
            Projectile.direction = (int)rot;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PinkTorch, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.DD2_DrakinDeath, Projectile.position);
        }
    }
}
