using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.Collections;

namespace WarWolfWorks_Mod.NPCs
{
    public class StandingSamurai : ModNPC
    {
        public bool PlayerClose;
        public bool PlayerCloser;
        public bool frame4and5Started;
        public float currentTimerTime;
        public float AnimationTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }
        public float DustTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Standing Samurai");

            //The frame count for the enemy
            Main.npcFrameCount[npc.type] = 6; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }
        /*
        public override string Texture
        {
            get { return "WarWolfWorks_Mod/Images/StandingSamurai"; }
        }
        public override string[] AltTextures
        {
            get { return new[] { "WarWolfWorks_Mod/Images/StandingSamurai/StandingSamuraiAlt1" }; }
        }
        */
        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            npc.width = 20;
            npc.height = 42;
            //The Enemies max health
            npc.lifeMax = 250;
            //Enemy Damage and Defence
            npc.damage = 50;
            npc.defense = 10;
            //The sound the enemy makes upon hit or death
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            //The amount of money that is dropped (as a float?)
            npc.value = 10f;
            //How much Knockback is resisted
            npc.knockBackResist = 20f;
            npc.aiStyle = -1;
            //The animation type to use
            //animationType = NPCID.Werewolf;
            //First we get the NPC to Banner using the below.
            banner = Item.NPCtoBanner(NPCID.Zombie);
            //Then we link the banner to the banner item
            bannerItem = Item.BannerToItem(banner);

        }

        public override void AI() //this is where you program your AI
        {
            float speed = 0;
            AnimationTimer++;
            DustTimer++;
            npc.TargetClosest(faceTarget: true);
            Player player = Main.player[npc.target];
            //Vector2 awayFrom = player.position - npc.position;
            Vector2 moveTo = player.Center; //This player is the same that was retrieved in the targeting section.
            MakeDustBlue();
            if (npc.velocity.Y != 0f) //The NPC is falling or jumping
            {
                speed = 10; //redundant to do it but i will keep it in case
            }
            if (npc.Distance(player.position) < 180)
            {
                npc.aiStyle = 3;
                animationType = NPCID.Zombie;
                if (player.position.X < npc.position.X) // if the target is to my left
                {
                    npc.spriteDirection = -1; // then go left
                    //npc.velocity.X -= 0.05f;
                }
                if (player.position.X > npc.position.X) // if the target is to my left
                {
                    npc.spriteDirection = 1; // then go right
                    //npc.velocity.X += 0.05f;
                }
                
                PlayerClose = true;
                speed = 1;
                if(npc.Distance(player.position) < 100)
                {
                    animationType = 0;
                    PlayerCloser = true;
                    speed += 4f; //make this whatever you want
                    Vector2 move = moveTo - npc.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > speed)
                    {
                        move *= speed / magnitude;
                    }
                    npc.velocity = move;
                }

                /* //CONSTANT SPEED
                Vector2 move = moveTo - npc.Center; //this is how much your npc wants to move
                float magnitude = (float)Math.Sqrt((move.X * move.X) + (move.Y * move.Y));
                move *= speed / magnitude; //this adjusts your npc's speed so that its speed is always constant
                npc.velocity = move;
                */
            }
            else
            {
                animationType = 0;
                npc.aiStyle = -1;
                PlayerClose = false;
                PlayerCloser = false;
                frame4and5Started = false;
                speed = 0;
                Vector2 move = moveTo - npc.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                npc.velocity = move;
            }
        }

        public void MakeDust()
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 56, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;
            }
            int gore = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y - 10f), npc.Center, 99, npc.scale); //99 is the ID for the smoke gore
            Main.gore[gore].velocity *= 0.3f;
            gore = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), npc.Center, 99, npc.scale);
            Main.gore[gore].velocity *= 0.3f;
            gore = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), npc.Center, 99, npc.scale);
            Main.gore[gore].velocity *= 0.3f;
        }

        public void MakeDustBlue()
        {
            if (Main.rand.Next(15) == 0)
            {
                int dustnumber = Dust.NewDust((npc.position - new Vector2(0,-20)), 15, 15, 56, 0f, 0f, 200, default(Color), 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
        }

        public override void FindFrame(int frameSize)
        {
            npc.frameCounter++;
            if (npc.velocity.X == 0f) //The NPC is standing
            {
                npc.frame.Y = 5 * frameSize;
                npc.frameCounter = 0.0;
            }
            if (PlayerCloser)
            {
                //For now i will use random system here because setting up each frame for animation flow for only 2 frames is redundant
                Random rnd = new Random();
                int i = rnd.Next(0,10);
                if (i >= 5)
                {
                    npc.frame.Y = 3 * frameSize;
                }
                else
                {
                    MakeDust();
                    npc.frame.Y = 4 * frameSize;

                    npc.frameCounter = 0;
                    PlayerCloser = false;
                }
                //npc.frameCounter += 1.0; //This makes the animation run
                //int frame = (int)(npc.frameCounter / 2.0) + 2;
            }
            /*
            if (PlayerClose)
            {
                if (AnimationTimer >= currentTimerTime)
                {
                    currentTimerTime = AnimationTimer + 30;
                    npc.frame.Y = 0;
                }
                else if (AnimationTimer >= currentTimerTime)
                {
                    npc.frame.Y = 2 * frameSize;
                    currentTimerTime = 0;
                    PlayerClose = false;
                    AnimationTimer = 0;
                }
            }
            */
        }
        
        private void RotateNPCToTarget()
        {
            Player player = Main.player[npc.target];
            if (player == null) return;
            Vector2 direction = npc.Center - player.Center;
        }
        

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //This is a way of spawning an enemy.
            //SpawnCondition contains a few options and chance gives the chance
            //You can return any float
            return SpawnCondition.OverworldNightMonster.Chance * 0.10f;
            //You can modify this to offer different scenarios.
            //For example:
            /*
             * float chance = 0f;
             * if(!Main.dayTime)
             * {
             *      chance += .1f;
             *      if(spawnInfo.spawnTileY <= Main.rockLayer && spawnInfo.spawnTileY >= Main.worldSurface * 0.15f)
             *      {
             *          chance += .2f;
             *      }
             * }
             * return chance;
             * */
            //In the above example we set a float chance to 0. We then increase it based on conditions.
            //First we check if it is night. If it is, we increase by .1 then we check if the y is between
            //Main.rockLayer and a bit above WorldSurface. If it is then we add .2.
            //In this example, the enemy is more likely to spawn on surface and underground but can spawn anywhere
            //if it is night.
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            npc.knockBackResist += 10;
            //This will loop and create dust upon being hit.
            for (int i = 0; i<10; i++)
            {
                int dustType = DustID.Blood; //OR A MOD SPECIFIC WOULD BE -----> DustType("WWWDustStandingSamurai");
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override void NPCLoot()
        {
            //This will make the NPC always drop this item
            Item.NewItem(npc.position, ItemID.DemoniteOre, Main.rand.Next(3));
            Item.NewItem(npc.position, mod.ItemType("LeatherPlate"), Main.rand.Next(3));
            //This will make the NPC only drop this item 25% of the time
            if (Main.rand.Next(4) == 0)
            {
                Item.NewItem(npc.position, ItemID.DemoniteBar, 1);
            }
            //This will make the NPC only drop in hardmode
            if (Main.hardMode)
            {
                Item.NewItem(npc.position, ItemID.Duck, 1);
            }
        }
    }
}
