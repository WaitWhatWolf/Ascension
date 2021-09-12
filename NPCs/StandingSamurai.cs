using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    public class StandingSamurai : AscensionNPC
    {
        public bool PlayerClose;
        public bool PlayerCloser;
        public bool frame4and5Started;
        public float currentTimerTime;
        public float AnimationTimer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float DustTimer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Standing Samurai");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 6; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }
        /*
        public override string Texture
        {
            get { return "Compact/Images/StandingSamurai"; }
        }
        public override string[] AltTextures
        {
            get { return new[] { "Compact/Images/StandingSamurai/StandingSamuraiAlt1" }; }
        }
        */
        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 20;
            NPC.height = 42;
            //The Enemies max health
            NPC.lifeMax = 250;
            //Enemy Damage and Defence
            NPC.damage = 50;
            NPC.defense = 10;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            //The amount of money that is dropped (as a float?)
            NPC.value = 10f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.01f;
            NPC.aiStyle = -1;
            //The animation type to use
            //animationType = NPCID.Werewolf;
            //First we get the NPC to Banner using the below.
            Banner = Item.NPCtoBanner(NPCID.Zombie);
            //Then we link the banner to the banner item
            BannerItem = Item.BannerToItem(Banner);

        }

        public override void AI() //this is where you program your AI
        {
            //NPC.TargetClosest(true);
            float speed = 0;
            AnimationTimer++;
            DustTimer++;
            NPC.TargetClosest(faceTarget: true);
            Player player = Main.player[NPC.target];
            //Vector2 awayFrom = player.position - NPC.position;
            Vector2 moveTo = player.Center; //This player is the same that was retrieved in the targeting section.
            MakeDustBlue();
            if (NPC.velocity.Y != 0f) //The NPC is falling or jumping
            {
                speed = 10; //redundant to do it but i will keep it in case
            }
            if (NPC.Distance(player.position) < 180)
            {
                NPC.aiStyle = 3;
                AnimationType = NPCID.Zombie;
                if (player.position.X < NPC.position.X) // if the target is to my left
                {
                    NPC.spriteDirection = -1; // then go left
                    //NPC.velocity.X -= 0.05f;
                }
                if (player.position.X > NPC.position.X) // if the target is to my left
                {
                    NPC.spriteDirection = 1; // then go right
                    //NPC.velocity.X += 0.05f;
                }
                
                PlayerClose = true;
                speed = 1;
                if(NPC.Distance(player.position) < 100)
                {
                    AnimationType = 0;
                    PlayerCloser = true;
                    speed += 4f; //make this whatever you want
                    Vector2 move = moveTo - NPC.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > speed)
                    {
                        move *= speed / magnitude;
                    }
                    NPC.velocity = move;
                }

                /* //CONSTANT SPEED
                Vector2 move = moveTo - NPC.Center; //this is how much your NPC wants to move
                float magnitude = (float)Math.Sqrt((move.X * move.X) + (move.Y * move.Y));
                move *= speed / magnitude; //this adjusts your NPC's speed so that its speed is always constant
                NPC.velocity = move;
                */
            }
            else
            {
                AnimationType = 0;
                NPC.aiStyle = -1;
                PlayerClose = false;
                PlayerCloser = false;
                frame4and5Started = false;
                speed = 0;
                Vector2 move = moveTo - NPC.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                NPC.velocity = move;
            }
        }

        public void MakeDust()
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
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

        public void MakeDustBlue()
        {
            if (Main.rand.Next(15) == 0)
            {
                int dustnumber = Dust.NewDust((NPC.position - new Vector2(0,-20)), 15, 15, 56, 0f, 0f, 200, default(Color), 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if (NPC.velocity.X == 0f) //The NPC is standing
            {
                NPC.frame.Y = 5 * frameSize;
                NPC.frameCounter = 0.0;
            }
            if (PlayerCloser)
            {
                //For now i will use random system here because setting up each frame for animation flow for only 2 frames is redundant
                Random rnd = new Random();
                int i = rnd.Next(0,10);
                if (i >= 5)
                {
                    NPC.frame.Y = 3 * frameSize;
                }
                else
                {
                    MakeDust();
                    NPC.frame.Y = 4 * frameSize;

                    NPC.frameCounter = 0;
                    PlayerCloser = false;
                }
                //NPC.frameCounter += 1.0; //This makes the animation run
                //int frame = (int)(NPC.frameCounter / 2.0) + 2;
            }
            /*
            if (PlayerClose)
            {
                if (AnimationTimer >= currentTimerTime)
                {
                    currentTimerTime = AnimationTimer + 30;
                    NPC.frame.Y = 0;
                }
                else if (AnimationTimer >= currentTimerTime)
                {
                    NPC.frame.Y = 2 * frameSize;
                    currentTimerTime = 0;
                    PlayerClose = false;
                    AnimationTimer = 0;
                }
            }
            */
        }
        
        private void RotateNPCToTarget()
        {
            Player player = Main.player[NPC.target];
            if (player == null) return;
            Vector2 direction = NPC.Center - player.Center;
        }
        

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //This is a way of spawning an enemy.
            //SpawnCondition contains a few options and chance gives the chance
            //You can return any float
            //return SpawnCondition.OverworldNightMonster.Chance * 0.10f;
            return SpawnCondition.Underground.Chance * 0.05f;
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
            //This will loop and create dust upon being hit.
            for (int i = 0; i<10; i++)
            {
                int dustType = DustID.Blood; //OR A MOD SPECIFIC WOULD BE -----> DustType("WWWDustStandingSamurai");
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override bool CheckDead()
        {
            NPC.NPCLoot();
            return base.CheckDead();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.DemoniteOre, 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeatherPlate>(), 1, 1, 3));
        }

    }
}
