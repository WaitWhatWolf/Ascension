using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Items.Projectiles;
using Ascension.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class DemonSwordOccultist : AscensionNPC
    {
        bool PlayerClose;
        bool SwingingSword;
        int counter;
        int counter2;
        int counter3;
        int counter4;
        bool justBursted;
        public bool wait = false;
        public bool withinMaxRange;
        int counterForLife;
        public static bool resistantToSmash = true;
        
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Demon Sword Occultist");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 8; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }
        public override void DrawBehind(int index)
        {
            MakeDustWeak();
            base.DrawBehind(index);
        }

        public override void SetDefaults()
        {
            NPC.boss = false;
            //Music = 0;
            //Enemy Hitbox Width and Height
            NPC.width = 104;
            NPC.height = 78;
            //The Enemies max health
            NPC.lifeMax = 700;
            //Enemy Damage and Defence
            NPC.damage = 45;
            NPC.defense = 25;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            //The amount of money that is dropped (as a float?)
            NPC.value = 1000f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.01f;
            NPC.aiStyle = 22;
            NPC.stepSpeed = 20;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.soundDelay = -1;
            NPC.behindTiles = false;
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest(faceTarget: true);
            Player player = Main.player[NPC.target];
            if (NPC.Distance(player.position) < 1200)
            {
                Music = MusicID.Temple;
                withinMaxRange = true;
            }
            if (NPC.Distance(player.position) > 1200)
            {
                withinMaxRange = false;
            }
            counterForLife++;
            counter++;
            counter2++;
            counter3++;
            counter4++;
            if (PlayerClose == false && withinMaxRange) NPC.position.Y = NPC.position.Y - 2;
            if (!justBursted)
            {
                counter4 = 0;
            }
            if(counter >= 100 && withinMaxRange)
            {
                SwingingSword = true;
            }
            if (counter >= 120 && withinMaxRange)
            {
                SoundEngine.PlaySound(SoundID.Item10, NPC.position);
                SwingingSword = false;
                MakeDust();

                float projectileSpeed = 16f;
                int damage = 12;
                float knockBack = 0.01f;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 3)) * projectileSpeed;
                Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 2, velocity.X, velocity.Y, ModContent.ProjectileType<DemonSwordOccultistSlash>(), damage, knockBack, Main.myPlayer);
                counter = 0;
            }

            if (NPC.Distance(player.position) < 180 && counter3 >= 60 && !justBursted && withinMaxRange)
            {
                NPC.velocity *= 0;
                counter = 0;
                SwingingSword = false;
                PlayerClose = true;
                if(counter3 == 60)
                {
                    SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                }
                if (counter3 == 90)
                {
                    SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                }
                if (counter3 > 120)
                {
                    SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                    justBursted = true;
                    float projectileSpeed = 0f;
                    int damage = 80;
                    float knockBack = 0.01f;
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                    new Vector2(NPC.position.X, NPC.position.Y + 100)) * projectileSpeed;
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 2, velocity.X, velocity.Y, ModContent.ProjectileType<DemonSwordOccultistBurst>(), damage, knockBack, Main.myPlayer);
                    counter3 = 0;
                    counter4 = 0;
                }

            }

            if (counter4 >= 300)
            {
                justBursted = false;
            }
            if (NPC.Distance(player.position) >= 180)
            {
                PlayerClose = false;
                counter3 = 0;
            }

            if (!withinMaxRange && counterForLife > 10)
            {
                NPC.life = NPC.life + 1;
                counterForLife = 0;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            //This will loop and create dust upon being hit.
            for (int i = 0; i < 10; i++)
            {
                int dustType = DustID.Blood; //OR A MOD SPECIFIC WOULD BE -----> DustType("WWWDustStandingSamurai");
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public void MakeDust()
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkCrystalShard, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black
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

        public void MakeDustWeak()
        {
                int dust = Dust.NewDust(NPC.position, NPC.width + Main.rand.Next(-50, 51), NPC.height + Main.rand.Next(-50, 51), DustID.PinkCrystalShard, 0f, 0f, 50, default(Color), 1f); //56 blue or 54 black
                Main.dust[dust].velocity *= 1.2f;
                Main.dust[dust].noGravity = true;
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if ((!PlayerClose && !SwingingSword) || (justBursted && !SwingingSword && PlayerClose))
            {
                if (counter2 > 0 && counter2 < 9)
                {
                    NPC.frame.Y = 0 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 10 && counter2 < 19)
                {
                    NPC.frame.Y = 1 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 20 && counter2 < 29)
                {
                    NPC.frame.Y = 2 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 30 && counter2 < 39)
                {
                    NPC.frame.Y = 1 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 39)
                {
                    counter2 = 0;
                }
            }
            if (PlayerClose && !SwingingSword && !justBursted)
            {
                if (counter2 > 0 && counter2 < 14)
                {
                    NPC.frame.Y = 3 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 15 && counter2 < 30)
                {
                    NPC.frame.Y = 4 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 30 && counter2 < 45)
                {
                    NPC.frame.Y = 5 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 45)
                {
                    counter2 = 0;
                }
            }
            if (SwingingSword)
            {
                if (counter2 > 0 && counter2 < 9)
                {
                    NPC.frame.Y = 6 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 10 && counter2 < 19)
                {
                    NPC.frame.Y = 7 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 20)
                {
                    counter2 = 0;
                }
            }
        }



        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {

            return true;       //this make that the NPC does not have a health bar
        }

        public override void OnKill()
        {
            if (Main.rand.Next(6) == 0)
            {
                Item.NewItem(NPC.position, ModContent.ItemType<DemonSword>(), 1);
            }
            else if (Main.rand.Next(3) == 0)
            {
                Item.NewItem(NPC.position, ModContent.ItemType<PortalDemonWings>(), 1);
            }
            Item.NewItem(NPC.position, ModContent.ItemType<YggdrasilPact>(), 1);
        }
        /*
        public override void DrawEffects( ref Color drawColor)
        {
           
        }
        */
    }
}
