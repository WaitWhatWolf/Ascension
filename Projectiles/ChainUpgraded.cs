using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles;
using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.Adragon, "9/9/2021 3:14:12 PM")]
    public class ChainUpgraded : AscensionProjectile
    {
        int counter;
        bool shouldICount;
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 36;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 1;
            Projectile.alpha = 400;
        }

        public override void AI()
        {
            Projectile.alpha -= 15;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            /*
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            */
                if (counter == 0)
                {
                    shouldICount = true;
                }
                if (shouldICount == true)
                {
                    counter++;
                }
                if (counter > 45)
                {
                    Player player = Main.player[Projectile.owner];
                    float ProjectileSpeed = 0;
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                    new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * ProjectileSpeed;
                    Projectile.velocity = velocity;
                }
                if (counter > 240)
                {
                    Player player = Main.player[Projectile.owner];
                    float ProjectileSpeed = 15;

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                    new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * ProjectileSpeed;

                    Projectile.velocity = velocity;
                    /*
                    Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile.), Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height, velocity.X,
                    velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                    Kill(0);
                    */
                    shouldICount = false;
                    counter = 1;
                }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item40, Projectile.position);
        }
    }
}
