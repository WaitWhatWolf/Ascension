using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Items.Projectiles;
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
    class DemonPortal : AscensionNPC
    {
        int counter;
        int counterhover;
        int notAttackingCounter;
        bool demonSpawned;
        bool attacking;
        bool withinYrange;
        bool withinMaxRange;
        int counterForLife;
        bool firstTimeTriggered;
        bool movedUp;

        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Demon Portal");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 5; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }

        public override void DrawBehind(int index)
        {
            MakeDustWeak();
        }
        public void MakeDustWeak()
        {
                int dust = Dust.NewDust(NPC.position, NPC.width + Main.rand.Next(-50, 51), NPC.height + Main.rand.Next(-50, 51), DustID.PinkSlime, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
                Main.dust[dust].velocity *= 1.2f;
                Main.dust[dust].noGravity = true;
        }
        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 81;
            NPC.height = 107;
            //The Enemies max health
            NPC.lifeMax = 1400;
            //Enemy Damage and Defence
            NPC.damage = 50;
            NPC.defense = 12;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
            //The amount of money that is dropped (as a float?)
            NPC.value = 500f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.01f;
            NPC.aiStyle = 0;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.soundDelay = -1;
            NPC.behindTiles = false;
            NPC.boss = false;
            //Music = 0;
            Music = MusicID.Temple;
        }



        public override void AI()
        {
            counterhover++;
            counterForLife++;
            #region Hover
            if (counterhover < 89)
            {
                NPC.velocity.Y = -0.15f;
            }
            if(counterhover > 90  && counterhover <= 179)
            {
                NPC.velocity.Y = 0.15f;
            }
            if(counterhover > 180)
            {
                counterhover = 0;
            }
            #endregion
            if (movedUp == false)
            {
                NPC.position.Y = NPC.position.Y - 40;
                movedUp = true;
            }
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(faceTarget: false);
            #region Checks for music and aggro
            if (!firstTimeTriggered)
            {
                //music = 0;
            }
            if(NPC.life == NPC.lifeMax && NPC.Distance(player.position) > 1500)
            {
                firstTimeTriggered = false;
                Music = 0;
            }
            if (NPC.Distance(player.position) < 1200)
            {
                withinMaxRange = true;
            }
            if (NPC.Distance(player.position) < 1200 && !firstTimeTriggered && NPC.life != NPC.lifeMax)
            {
                //Main.PlaySound(SoundLoader.customSoundType, -1, -1, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/POEHEIST"));
                // Custom Music: music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DriveMusic" //FOR BOSSES
                Music = MusicID.Temple;
                firstTimeTriggered = true;
            }
            if (NPC.Distance(player.position) < 1200 && NPC.life != NPC.lifeMax)
            {
                Music = MusicID.Temple;
                withinMaxRange = true;
            }
            if (NPC.Distance(player.position) < 1200 && firstTimeTriggered)
            {
                Music = MusicID.Temple;
                withinMaxRange = true;
            }
            if (player.position.Y+ 120 >= NPC.position.Y || player.position.Y + 50 >= NPC.position.Y)
            {
                withinYrange = false;
            }
            if (player.position.Y + 120 <= NPC.position.Y || player.position.Y + 50 <= NPC.position.Y)
            {
                withinYrange = true;
            }
            #endregion
            if(firstTimeTriggered)
            {
                counter++;
            }
            if(counter >= 300 && withinMaxRange && NPC.life != NPC.lifeMax || counter >= 300 && withinMaxRange && NPC.life == NPC.lifeMax && firstTimeTriggered)
            {
                attacking = true;
            }
            if(counter >= 420 && withinMaxRange && NPC.life != NPC.lifeMax || counter >= 300 && withinMaxRange && NPC.life == NPC.lifeMax && firstTimeTriggered)
            {
                int i = 1;
                SoundEngine.PlaySound(SoundID.Item30, NPC.position);
                NPC.life = NPC.life + 20;
                float projectileSpeed = 2f;
                int damage = 30;
                float knockBack = 0.01f;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width, NPC.position.Y + NPC.height)) * projectileSpeed;
                for(int k = 0; k < 10; k++)
                {
                    if (withinYrange)
                    {
                        if (i >= 6)
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width + (20 * i) , NPC.position.Y + NPC.height, velocity.X, velocity.Y, ModContent.ProjectileType<DemonPortalWave>(), damage, knockBack, Main.myPlayer);
                        else
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width + (20 * i), NPC.position.Y + NPC.height, velocity.X, velocity.Y, ModContent.ProjectileType<DemonPortalWave>(), damage, knockBack, Main.myPlayer);
                    }
                    if(!withinYrange)
                    {
                        if (i >= 6)
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height + (20 * i) -90, velocity.X, velocity.Y, ModContent.ProjectileType<DemonPortalWave>(), damage, knockBack, Main.myPlayer);
                        else
                            Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width, NPC.position.Y + NPC.height - (20 * i) -70, velocity.X, velocity.Y, ModContent.ProjectileType<DemonPortalWave>(), damage, knockBack, Main.myPlayer);

                    }
                    i++;
                    if (i > 10)
                    {
                        i = 1;
                    }
                }
                attacking = false;
                counter = 0;
            }
            if(!attacking)
            {
                notAttackingCounter++;
            }
            if(NPC.life < (NPC.lifeMax/1.5) && !demonSpawned)
            {
                SoundEngine.PlaySound(SoundID.FemaleHit, NPC.position);
                Random rnd = new Random();
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(50, 100)), (int)NPC.Center.Y + (rnd.Next(50, 100)), (int)ModContent.NPCType<DemonSwordOccultist>());
                demonSpawned = true;
            }

            if(!withinMaxRange && counterForLife > 10)
            {
                NPC.life = NPC.life + 1;
                counterForLife = 0;
            }
        }


        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if(!attacking)
            {
                if(notAttackingCounter < 19)
                {
                    NPC.frame.Y = 0 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (notAttackingCounter < 39 && notAttackingCounter >= 19)
                {
                    NPC.frame.Y = 1 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (notAttackingCounter >= 39 && notAttackingCounter < 59)
                {
                    NPC.frame.Y = 2 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (notAttackingCounter >= 59)
                {
                    NPC.frame.Y = 3 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (notAttackingCounter >= 79)
                {
                    notAttackingCounter = 0;
                }
            }
            if(attacking == true)
            {
                NPC.frame.Y = 4 * frameSize;
                NPC.frameCounter = 0.0;
            }

        }



        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {

            return true;       //this make that the NPC does not have a health bar
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3 == true && spawnInfo.player.ZoneCorrupt || NPC.downedBoss3 == true && spawnInfo.player.ZoneCrimson)
            {
                return SpawnCondition.Overworld.Chance * 0.01f;
            }
            else
                return 0;
        }
        /* //old
        public override void NPCLoot()
        {
            Item.NewItem(NPC.position, ItemType("PortalCatalyst"), 1);
        }
        */
        public override void OnKill()
        {
            Item.NewItem(NPC.position, ModContent.ItemType<PortalCatalyst>(), 1);
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            //music = MusicID.Temple;
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            //music = MusicID.Temple;
        }
    }
}
