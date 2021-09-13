using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Items.Weapons;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
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
        bool initLinkChain;
        int linkChainCounter2;
        bool secondSpawn;
        bool entryLine;

        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Prophet of the Sword Occult");
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
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
            NPC.lifeMax = 9500;
            //Enemy Damage and Defence
            NPC.damage = 30;
            NPC.defense = 18;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath49;
            //The amount of money that is dropped (as a float?)
            NPC.value = 1000f;
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.knockBackResist = 0.01f;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.behindTiles = false;
            NPC.lavaImmune = true;
            NPC.buffImmune[24] = true;
            //LEAVING THOSE BELOW AFTER THE TMODLOADER FIXES SO I CAN TEST AROUND
            //Music = Mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Assets/Sound/Custom/ProphetBossBattle");
            //SoundEngine.PlaySound(Mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Assets/Sound/Custom/ProphetBossBattle"), NPC.position);
            //Music = MusicLoader.GetMusicSlot(ASCResources.ASSETS_PATH_SOUND_CUSTOM + "ProphetBossBattle");
            Music = MusicLoader.GetMusicSlot(ASCResources.ASSETS_PATH_SOUND + ASCResources.ASSETS_SUBPATH_MUSIC + "ProphetBossBattle");
        }
        public override void AI()
        {
            counterForChainCirclet++;
            counterForSingleChain++;
            counterForMeleeAttack++;
            animationCounter++;
            if(entryLine == false)
            {
                entryLine = true;
                Talk("Shouldn't have used that!");
            }
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
            #region Escape from battle
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            #endregion
            #region Life Check
            if (NPC.life < (NPC.lifeMax / 1.5) && !firstSpawn)
            {
                Talk("Minions come forth!");
                firstSpawn = true;
                SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
                Random rnd = new Random();
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DesertSwordOccultist>());
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DesertSwordOccultist>());
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DesertSwordOccultist>());
            }
            if (NPC.life < (NPC.lifeMax / 2) && phase == 1 && !secondSpawn)
            {
                Talk("Azazel grant me power!");
                secondSpawn = true;
                phase = 2;
                SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
                Random rnd = new Random();
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<SwordOccultist>());
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
                int damage = 15;

                if (phase == 2)
                {
                    damage = (int)(damage * 1.5f);
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
                    SoundEngine.PlaySound(SoundID.Item7, NPC.position);
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X,
                    velocity.Y, ModContent.ProjectileType<Chain>(), damage, knockBack, Main.myPlayer);
                }
                if (phase == 2)
                {
                    SoundEngine.PlaySound(SoundID.Item7, NPC.position);
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height, velocity.X,
                    velocity.Y, ModContent.ProjectileType<ChainUpgraded>(), damage, knockBack, Main.myPlayer);
                }
                
            }
            #endregion
            #region Circlet Chain
            if (counterForChainCirclet > 600)
            {
                int damage = 12;
                counterForChainCirclet = 0;
                attacking = true;
                animationCounter = 0;
                if (phase == 2)
                {
                    damage = (int)(damage * 1.5f);
                }
                float knockBack = 0.1f;
                float projectileSpeed = 4;
                if (phase == 2)
                {
                    projectileSpeed = projectileSpeed * 1.5f;
                }
                SoundEngine.PlaySound(SoundID.Item28, NPC.position);
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
                float speed = 8f;
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
                    if (counterForMeleeAttack >= 600)
                    {
                        meleeAttack = false;
                        counterForMeleeAttack = 0;
                        //NPC.aiStyle = 0;
                    }
                }
                if (phase == 2)
                {
                    if (counterForMeleeAttack >= 600)
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
                int damage = 12;
                SoundEngine.PlaySound(SoundID.Item121, NPC.position);
                linkChainCounter2++;
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
        private void Talk(string message)
        {
            if (Main.netMode != NetmodeID.Server)
            {               
                string text = Language.GetTextValue(message,Lang.GetNPCNameValue(NPC.type));
                Main.NewText(text, 133, 56, 93);
            }
            else
            {
                NetworkText text = NetworkText.FromKey(message,Lang.GetNPCNameValue(NPC.type));
                Terraria.Chat.ChatHelper.BroadcastChatMessage(text, new Color(133, 56, 93));
            }
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
                        SoundEngine.PlaySound(SoundID.Item31, NPC.position);
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
                        SoundEngine.PlaySound(SoundID.Item31, NPC.position);
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
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
        public override bool PreKill()
        {            
            return true;
        }
        public override void OnKill()
        {
            //Talk("Im sorry..failed you..blegh"); Dont need it anymore unless stuff breaks again
        }

        public override bool CheckDead()
        {
            if (base.CheckDead())
            {
                Talk("Im sorry..failed you..blegh");
                NPC.NPCLoot();
                return true;
            }

            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int[] ItemType = { ModContent.ItemType<ChainsOfHate>(), ModContent.ItemType<ChainsOfJustice>() };
            npcLoot.Add(ItemDropRule.OneFromOptions(1,ItemType));
        }
    }
}
