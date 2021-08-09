using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class DesertSwordOccultist : AscensionNPC
    {
        bool PlayerClose;
        int counter;
        int counter2;
        public bool wait = false;
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Silent Sword Occultist");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 3; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 38;
            NPC.height = 44;
            //The Enemies max health
            NPC.lifeMax = 80;
            //Enemy Damage and Defence
            NPC.damage = 22;
            NPC.defense = 15;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath5;
            //The amount of money that is dropped (as a float?)
            NPC.value = 1000f;
            //How much Knockback is resisted
            NPC.knockBackResist = -1f;
            NPC.aiStyle = 6;
            NPC.stepSpeed = 20;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.soundDelay = -1;
            NPC.behindTiles = false;
        }


        public override void AI()
        {
            Player player = Main.player[NPC.target];
            counter++;
            counter2++;
            
            if (NPC.alpha <= 210 && wait == false)
            {
                NPC.alpha += 10;
                if (NPC.alpha > 210)
                {
                    counter = 0;
                    wait = true;

                }
            }
            if (wait == true)
            {
                if (counter > 90)
                {
                    NPC.alpha -= 10;
                    if (NPC.alpha < 10)
                    {
                        wait = false;
                    }
                }
            }
            
            if (NPC.Distance(player.position) < 180)
            {
                PlayerClose = true;
                NPC.alpha = 0;
            }
            if (NPC.Distance(player.position) >= 180)
            {
                PlayerClose = false;
            }
        }
        
        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if (!PlayerClose)
            {
                if (counter2 > 0 && counter2 <14)
                {
                    NPC.frame.Y = 0 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 15)
                    NPC.frame.Y = 1 * frameSize;
                    NPC.frameCounter = 0.0;
                if (counter2 >= 30)
                {
                    counter2 = 0;
                }

            }
            if (PlayerClose)
            {
                NPC.frame.Y = 2 * frameSize;
                NPC.frameCounter = 0.0;
            }
        }
        


        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {

            return false;       //this make that the NPC does not have a health bar
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3 == true)
            {
                return SpawnCondition.DesertCave.Chance * 0.05f;
            }
            else
            return SpawnCondition.DesertCave.Chance * 0.0f;
        }

        public override void OnKill()
        {
            if (Main.rand.Next(6) == 0)
            {
                Item.NewItem(NPC.position, ModContent.ItemType<SilentMinionItem>(), 1);
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.DD2_GoblinHurt, NPC.position);
            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                NPC.NewNPC((int)NPC.Center.X+(rnd.Next(30,100)), (int)NPC.Center.Y +(rnd.Next(30, 100)), (int)ModContent.NPCType<SilentMinionEnemy>());
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.DD2_GoblinHurt, NPC.position);
            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                NPC.NewNPC((int)NPC.Center.X + (rnd.Next(30, 100)), (int)NPC.Center.Y +(rnd.Next(30, 100)), (int)ModContent.NPCType<SilentMinionEnemy>());
            }
        }


        /*
        public override void DrawEffects( ref Color drawColor)
        {
           
        }
        */
    }
}
