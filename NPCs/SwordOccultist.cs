using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Items.Projectiles;
using Ascension.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SwordOccultist : AscensionNPC
    {
        int counter = 0;
        int counter2 = 0;
        int counter3 = 0;
        bool Shooting = false;
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Dungeon Sword Occultist");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 3; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 38;
            NPC.height = 46;
            //The Enemies max health
            NPC.lifeMax = 100;
            //Enemy Damage and Defence
            NPC.damage = 35;
            NPC.defense = 20;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            //The amount of money that is dropped (as a float?)
            NPC.value = 10f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = -1;
            ModContent.ProjectileType<SmallPurpleFlame>();
        }

        public override void AI() //this is where you program your AI
        {
            //NPC.frame.Y = 0;
            NPC.velocity.X = 0;
            counter++;
            counter2++;
            counter3++;
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest(faceTarget: true);
            Player player = Main.player[NPC.target];
            NPC.noTileCollide = false;
            int num184 = (int)(NPC.Center.X / 16f);
            int num185 = (int)(NPC.Center.Y / 16f);
            if (NPC.Distance(player.position) < 180)
            {
                Teleport();
            }
            if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].IsActiveUnactuated && (Main.tileSolid[(int)Main.tile[num184, num185].type] || Main.tileSolidTop[(int)Main.tile[num184, num185].type])))
            {
                Teleport();
            }
            if (counter2 > 180)
            {
                Teleport();
                counter2 = 0;
            }
            else
            {
                if (counter > 20 && NPC.Distance(player.position) < 1000)
                {
                    SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                    counter3 = 0;
                    Shooting = true;
                    MakeDust();
                    float projectileSpeed = 6f;
                    int damage = 12;
                    float knockBack =  0.01f;
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                    new Vector2(NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 3)) * projectileSpeed;
                    Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.position.X + NPC.width / 2, NPC.position.Y + NPC.height / 2, velocity.X, velocity.Y, ModContent.ProjectileType<SmallPurpleFlame>(), damage, knockBack, Main.myPlayer);
                    counter = 0;
                }
            }
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if (NPC.velocity.X == 0f && !Shooting) //The NPC is standing
            {
                NPC.frame.Y = 0 * frameSize;
                NPC.frameCounter = 0.0;
            }
            if (Shooting)
            {
                NPC.frame.Y = 1 * frameSize;
                NPC.frameCounter = 0;
            }
            if (counter3 > 15)
            {
                NPC.frame.Y = 0 * frameSize;
                NPC.frameCounter = 0.0;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            NPC.knockBackResist += 10;
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


        public void Teleport()
        {
            SoundEngine.PlaySound(SoundID.Item25, NPC.position);
            Player player = Main.player[NPC.target];
            NPC.position.X = player.position.X + Main.rand.Next(-300,300);
            NPC.position.Y = player.position.Y - Main.rand.Next(-300,300); ;
            NPC.netUpdate = true;
            MakeDust();
        }

        public void MakeDust()
        {
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 27, 0f, 0f, 50, default(Color), 1.5f); //56 blue or 54 black 27 purple
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


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Dungeon.Chance * 0.03f;
        }

        public override bool CheckDead()
        {
            NPC.NPCLoot();
            return base.CheckDead();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneBlade>(), 6, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<JudgementChainPact>(), 1, 1, 1));
        }
    }
}
