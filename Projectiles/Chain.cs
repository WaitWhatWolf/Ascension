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
    [CreatedBy(Dev.Adragon, "9/8/2021 4:11:53 PM")]
    public class Chain : AscensionProjectile
    {
        int counter;
        bool shouldICount;
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 3;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 1;
        }

        public override void AI()
        {

            //Projectile.TargetClosest(faceTarget: true);
            //Projectile.FindTargetWithinRange
            //Player player = Main.player[owner.target];
            //Projectile target = Main.player[i];
            if(counter == 0)
            {
                shouldICount = true;
            }
            if(shouldICount == true)
            {
                counter++;
            }
            if(counter > 60)
            {
                Player player = Main.player[Projectile.owner];
                float projectileSpeed = 0;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * projectileSpeed;
                Projectile.velocity = velocity;
            }
            if(counter > 300)
            {
                Player player = Main.player[Projectile.owner];
                float projectileSpeed = 15;

                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * projectileSpeed;

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
