using Ascension.Buffs;
using Ascension.Items;
using Ascension.Items.Projectiles;
using Ascension.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Ascension.NPCs
{
    class BulkySwordOccultist : AscensionNPC
    {
        int counter; //animation
        int counter2; //cd
        int counter3;
        int counter4;
        int counterToDeactivate;
        bool PlayerClose;
        bool swinging;
        int animationSpeed;
        Player player2;
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Bulky Sword Occultist");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 6; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 40;
            NPC.height = 95;
            //The Enemies max health
            NPC.lifeMax = 250;
            //Enemy Damage and Defence
            NPC.damage = 25;
            NPC.defense = 25;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.DD2_DrakinDeath;
            //The amount of money that is dropped (as a float?)
            NPC.value = 10f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = 3;
            NPC.noGravity = false;
            NPC.netAlways = true;
            ModContent.ProjectileType<BulkySmashWave>();
            animationSpeed = 1;
        }
        public override bool CheckActive()
        {
            if (counterToDeactivate < 600)
            {
                return false;
            }
            if (counterToDeactivate > 600)
            {
                return true;
            }
            else
                return false;
        }

        public override void AI()
        {
            counter2++; //anim
            counter4++;
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            player2 = player;
            NPC.TargetClosest(faceTarget: true);
            if (NPC.Distance(player.position) < 1300)
            {
                PlayerClose = true;
            }
            if (NPC.Distance(player.position) > 1300)
            {
                animationSpeed = 1;
                counterToDeactivate++;
                counter3 = 0;
                NPC.velocity.X = NPC.velocity.X * 0.8f;
                PlayerClose = false;
            }
            if (NPC.Distance(player.position) > 500)
            {
                NPC.velocity.X = NPC.velocity.X * 1.01f;
                animationSpeed = 2;
            }
            if (NPC.Distance(player.position) < 499)
            {
                animationSpeed = 1;
                counter3 = 0;
                NPC.velocity.X = NPC.velocity.X * 0.8f;
            }

            if (NPC.Distance(player.position) < 100)
            {
                NPC.velocity.X = 0;
            }

            if (NPC.Distance(player.position) < 1100 && counter4 > 100)
            {
                NPC.aiStyle = -1;
                counter++;
                swinging = true;
                if (animationSpeed != 2)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.5f;
                    NPC.aiStyle = -1;
                }
            }

            if (counter >= 75 && NPC.Distance(player.position) < 1100)
            {
                counter4 = 0;
                NPC.aiStyle = 3;
                SoundEngine.PlaySound(SoundID.Item7, NPC.position);
                swinging = false;
                    float projectileSpeed = 12f;
                    int damage = 14;
                    float knockBack = 0.1f;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X, player.position.Y) -
                new Vector2(NPC.position.X + NPC.width, NPC.position.Y)) * projectileSpeed;
                Projectile.NewProjectile(new ProjectileSource_NPC(NPC),NPC.position.X + NPC.width, NPC.position.Y - (NPC.height/3), velocity.X * 2, velocity.Y / 2, ModContent.ProjectileType<BulkySmashWave>(), damage, knockBack, Main.myPlayer);
                counter = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            player2.AddBuff(ModContent.BuffType<Smash>(), 1, true);
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if (!swinging)
            {
                if (counter2 > 0 && counter2 < 24 / animationSpeed && PlayerClose)
                {
                    NPC.frame.Y = 1 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 25 / animationSpeed && counter2 < 49 / animationSpeed && PlayerClose)
                {
                    NPC.frame.Y = 2 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 50 / animationSpeed && PlayerClose)
                {
                    NPC.frameCounter = 0.0;
                    counter2 = 0;
                }
            }

            if (swinging)
            {
                if (counter2 > 0 && counter2 < 24)
                {
                    NPC.frame.Y = 3 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 25 && counter2 < 49)
                {
                    NPC.frame.Y = 4 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 50 && counter2 < 74)
                {
                    NPC.frame.Y = 5 * frameSize;
                    NPC.frameCounter = 0.0;
                }
                if (counter2 >= 75)
                {
                    counter2 = 0;
                    NPC.frameCounter = 0.0;
                }
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3 == true)
            {
                return SpawnCondition.Overworld.Chance * 0.05f;
            }
            else
                return SpawnCondition.Overworld.Chance * 0.0f;
        }

        public override bool CheckDead()
        {
            NPC.NPCLoot();
            return base.CheckDead();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Zanbato>(), 6, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<KusanagiPact>(), 1, 1, 1));
        }
    }
}
