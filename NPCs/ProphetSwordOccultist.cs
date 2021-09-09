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
        int timesHit;
        int timesHit2; //counter for teleport
        int i = 1;
        int j = 1;
        int counterForChainCirclet;
        int counterForSingleChain;
        int counterAfterHit;
        int counterForMeleeAttack;
        bool gotHit;
        float randomDirection;
        bool meleeAttack;
        bool teleport;
        bool attacking;
        int animationCounter;
        int phase = 1;
        bool firstSpawn;
        int linkChainCounter;
        int linkChain;
        bool initLinkChain;
        int linkChainCounter2;
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
            counterForSingleChain++;
            counterForMeleeAttack++;
            animationCounter++;
            if (phase == 2)
            {
                linkChainCounter++;
            }
            if(gotHit == true)
            {
                counterAfterHit++;
                if (counterAfterHit >= 60)
                {
                    counterAfterHit = 0;
                    gotHit = false;
                }
            }
            NPC.TargetClosest(faceTarget: true);
            Player player = Main.player[NPC.target];
            #region Life Check
            if (NPC.life < (NPC.lifeMax / 1.5) && !firstSpawn)
            {
                firstSpawn = true;
                SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
                Random rnd = new Random();
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DesertSwordOccultist>());
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DesertSwordOccultist>());
            }
            if (NPC.life < (NPC.lifeMax / 2) && phase == 1)
            {
                phase = 2;
                SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
                Random rnd = new Random();
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<SwordOccultist>());
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<SwordOccultist>());
            }
            #endregion
            #region Movement
            if (gotHit == false && meleeAttack == false)
            {
                Vector2 moveTo = player.Center + new Vector2(-3f, -300f);
                float speed = 6f;
                if (phase == 2)
                {
                    speed += 3;
                }
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
            }
            if(gotHit == true && meleeAttack == false)
            {
                int i = Main.rand.Next(2);
                if(i == 0)
                {
                    randomDirection = 300f;
                }
                if(i == 1)
                {
                    randomDirection = -300f;
                }
                Vector2 moveTo = player.Center + new Vector2(randomDirection, -300f);
                float speed = 9f;
                if (phase == 2)
                {
                    speed += 3;
                }
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
            }
            if(teleport == true)
            {
                teleport = false;
                SoundEngine.PlaySound(SoundID.NPCHit49, NPC.position);
                if (phase == 1)
                {
                    NPC.position.X = player.position.X + Main.rand.Next(-300, 300);
                    NPC.position.Y = player.position.Y - Main.rand.Next(-300, 300); ;
                }
                if (phase == 2)
                {
                    NPC.position.X = player.position.X + Main.rand.Next(-400, 400);
                    NPC.position.Y = player.position.Y - Main.rand.Next(-400, 400); ;
                }
                NPC.netUpdate = true;
            }
            #endregion
            #region Single Chain
            if (counterForSingleChain > 60)
            {
                counterForSingleChain = 0;
                int damage = 5;
                if(phase == 2)
                {
                    damage = damage * 3 / 2;
                }
                float knockBack = 0.1f;
                float projectileSpeed = 4;
                if (phase == 2)
                {
                    projectileSpeed = projectileSpeed * 1.5f;
                }
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width, NPC.position.Y + NPC.height)) * projectileSpeed;

                if (phase == 1)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X,
                    velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                }
                if (phase == 2)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X,
                    velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                }
                
            }
            #endregion
            #region Circlet Chain
            if (counterForChainCirclet > 600)
            {
                counterForChainCirclet = 0;
                attacking = true;
                animationCounter = 0;
                int damage = 5;
                if (phase == 2)
                {
                    damage = damage * 3 / 2;
                }
                float knockBack = 0.1f;
                float projectileSpeed = 4;
                if (phase == 2)
                {
                    projectileSpeed = projectileSpeed * 1.5f;
                }

                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width, NPC.position.Y + NPC.height)) * projectileSpeed;
                /*
                Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X, 
                velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                */
                if (phase == 1)
                {
                    int SpaceBy = 60;
                    for (int k = 0; k < 32; k++)
                    {
                        if (i <= 8)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width - (SpaceBy * j)) + 50, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
                               -velocity.X, -velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                        }
                        if (i <= 16 && i > 8)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width + (SpaceBy * j)) - 100, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
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
                        if (j > 8)
                        {
                            j = 1;
                        }
                    }
                }
                if (phase == 2)
                {
                    int SpaceBy = 80;
                    for (int k = 0; k < 32; k++)
                    {
                        if (i <= 8)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width - (SpaceBy * j)) + 50, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
                               -velocity.X, -velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                        }
                        if (i <= 16 && i > 8)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width + (SpaceBy * j)) - 100, (NPC.position.Y + NPC.height + (SpaceBy * j) - 200),
                               velocity.X, -velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                        }
                        if (i <= 24 && i > 16)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width + (SpaceBy * j)) - 400, (NPC.position.Y + NPC.height + (SpaceBy * j) - 600),
                               velocity.X, velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                        }
                        if (i <= 32 && i > 24)
                        {
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width - (SpaceBy * j)) + 400, (NPC.position.Y + NPC.height + (SpaceBy * j) - 600),
                               velocity.X, velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                        }
                        j++;
                        i++;
                        if (i > 32)
                        {
                            i = 1;
                        }
                        if (j > 8)
                        {
                            j = 1;
                        }
                    }
                }
            }
            #endregion
            #region Melee Attack
            if (counterForMeleeAttack >= 420)
            {
                meleeAttack = true;
                Vector2 moveTo = player.Center;
                float speed = 10f;
                if (phase == 2)
                {
                    speed = speed * 1.5f;
                }
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
                //NPC.aiStyle = 19;
                if (phase == 1)
                {
                    if (counterForMeleeAttack >= 780)
                    {
                        meleeAttack = false;
                        counterForMeleeAttack = 0;
                        //NPC.aiStyle = 0;
                    }
                }
                if (phase == 2)
                {
                    if (counterForMeleeAttack >= 660)
                    {
                        meleeAttack = false;
                        counterForMeleeAttack = 0;
                        //NPC.aiStyle = 0;
                    }
                }
            }
            #endregion
            #region Line Chain
            if (phase == 2 && linkChainCounter >= 600)
            {
                linkChainCounter = 0 + Main.rand.Next(0, 120);
                initLinkChain = true;
            }
            if(initLinkChain == true)
            {
                linkChainCounter2++;
                int damage = 5;
                float knockBack = -1f;
                float projectileSpeed = 2;

                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 2)) * projectileSpeed;
                if (linkChainCounter2 == 1)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 + 100), (NPC.position.Y + NPC.height / 2 + 600), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 2)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 + 100), (NPC.position.Y + NPC.height / 2 - 600), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 3)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 - 500), (NPC.position.Y + NPC.height / 2 + 600), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 4)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 - 500), (NPC.position.Y + NPC.height / 2 - 600), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 5)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 + 200), (NPC.position.Y + NPC.height / 2 - 200), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 6)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 + 200), (NPC.position.Y + NPC.height / 2 + 200), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 7)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 - 600), (NPC.position.Y + NPC.height / 2 + 200), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }
                if (linkChainCounter2 == 8)
                {
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), (NPC.position.X + NPC.width / 2 - 600), (NPC.position.Y + NPC.height / 2 - 200), velocity.X,
velocity.Y, ModContent.ProjectileType<LineChain>(), damage, knockBack, Main.myPlayer);
                }

                if (linkChainCounter2 >= 8)
                {
                    linkChainCounter2 = 0;
                    initLinkChain = false;
                }
            }

            #endregion
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if(phase == 1)
            {
                if (!attacking)
                {
                    if (animationCounter < 19)
                    {
                        NPC.frame.Y = 0 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 39 && animationCounter >= 19)
                    {
                        NPC.frame.Y = 1 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter >= 59)
                    {
                        animationCounter = 0;
                    }
                }
                if (attacking == true)
                {
                    if (animationCounter < 19)
                    {
                        NPC.frame.Y = 2 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 39 && animationCounter >= 19)
                    {
                        NPC.frame.Y = 3 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 300 && animationCounter >= 299)
                    {
                        NPC.frame.Y = 4 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter >= 420)
                    {
                        attacking = false;
                        animationCounter = 0;
                    }
                }
            }
            if(phase == 2)
            {
                if (!attacking)
                {
                    if (animationCounter < 19)
                    {
                        NPC.frame.Y = 5 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 39 && animationCounter >= 19)
                    {
                        NPC.frame.Y = 6 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter >= 59)
                    {
                        animationCounter = 0;
                    }
                }
                if (attacking == true)
                {
                    if (animationCounter < 19)
                    {
                        NPC.frame.Y = 7 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 39 && animationCounter >= 19)
                    {
                        NPC.frame.Y = 8 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter < 240 && animationCounter >= 239)
                    {
                        NPC.frame.Y = 9 * frameSize;
                        NPC.frameCounter = 0.0;
                    }
                    if (animationCounter >= 420)
                    {
                        attacking = false;
                        animationCounter = 0;
                    }
                }
            }
        }

        public void MakeDust()
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black 27 purple
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;
            }
            int gore = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y - 10f), NPC.Center, 99, NPC.scale); //99 is the ID for the smoke gore
            Main.gore[gore].velocity *= 0.3f;
            gore = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 15f), NPC.Center, 99, NPC.scale);
            Main.gore[gore].velocity *= 0.3f;
            gore = Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 20f), NPC.Center, 99, NPC.scale);
            Main.gore[gore].velocity *= 0.3f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            meleeAttack = false;
            counterForMeleeAttack = 0;
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            timesHit++;
            timesHit2++;
            if(timesHit > 3)
            {
                timesHit = 0;
                gotHit = true;
            }
            if(timesHit2 > 8)
            {
                timesHit2 = 0;
                teleport = true;
            }
            if (phase == 2)
            {
                timesHit++;
                timesHit2++;
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            counterForMeleeAttack += 120;

        }
    }
}
