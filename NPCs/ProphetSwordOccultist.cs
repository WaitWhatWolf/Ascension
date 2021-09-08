using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class ProphetSwordOccultist : AscensionNPC
    {
        int i = 1;
        int j = 1;
        int counterForChainCirclet;
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Prophet of Sword Occult");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 10; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.NPCs, "ProphetSwordOccultist/", this);

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 38;
            NPC.height = 46;
            //The Enemies max health
            NPC.lifeMax = 10000;
            //Enemy Damage and Defence
            NPC.damage = 50;
            NPC.defense = 16;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            //The amount of money that is dropped (as a float?)
            NPC.value = 1000f;
            NPC.aiStyle = -1;
            //mod.ProjectileType("SmallPurpleFlame");
            NPC.boss = true;
            NPC.knockBackResist = 0.01f;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.behindTiles = false;
            //Music = ModContent.GetSoundSlot(SoundType.Music, "Sounds/Music/POEHEISTWAV");
        }

        public override void AI()
        {
            counterForChainCirclet++;

            NPC.TargetClosest(faceTarget: true);
            Player player = Main.player[NPC.target];
            #region Movement
            Vector2 moveTo = player.Center + new Vector2(-3f, -300f);
            float speed = 9f;
            Vector2 move = moveTo - NPC.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 10f; //the larger this is, the slower the boss will turn
            move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            NPC.velocity = move;
            #endregion

            if (counterForChainCirclet > 60)
            {
                counterForChainCirclet = 0;
                int damage = 5;
                float knockBack = 0.1f;
                float projectileSpeed = 7;

                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width, NPC.position.Y + NPC.height)) * projectileSpeed;
                /*
                Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X, 
                velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                */
                int SpaceBy = 60;
                for (int k = 0; k < 32; k++)
                {
                    if (i <= 8)
                    {
                        Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width - (SpaceBy * j)) +50, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
                           -velocity.X, -velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                    }
                    if(i <= 16 && i > 8)
                    {
                        Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width + (SpaceBy * j)) -100, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
                           velocity.X, -velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                    }
                    if (i <= 24 && i > 16)
                    {
                        Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width + (SpaceBy * j)) - 400, (NPC.position.Y + NPC.height + (SpaceBy * j) - 600),
                           velocity.X, velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                    }
                    if (i <= 32 && i > 24)
                    {
                        Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width - (SpaceBy * j)) + 400, (NPC.position.Y + NPC.height + (SpaceBy * j) - 600),
                           velocity.X, velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                    }
                    j++;
                    i++;
                    if (i > 32)
                    {
                        i = 1;
                    }
                    if(j > 8)
                    {
                        j = 1;
                    }
                }
            }
        }
    }
}
