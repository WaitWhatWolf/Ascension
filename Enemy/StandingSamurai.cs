using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Enemy
{
    [AutoloadHead]
    public class WWWMEnemy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Standing Samurai");

            //The frame count for the enemy
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Zombie]; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)

        }

        public override string Texture
        {
            get { return "WarWolfWorks_Mod/Images/StandingSamurai"; }
        }

        public override string[] AltTextures
        {
            get { return new[] { "WarWolfWorks_Mod/Images/StandingSamurai/StandingSamurai_Alt_1" }; }
        }
        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            npc.width = 18;
            npc.height = 40;
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
            npc.knockBackResist = 99f;
            //The AI Style (Custom will use -1)
            npc.aiStyle = 0;
            //Similiar to AI style
            aiType = NPCID.PrimeSaw;
            //The animation type to use
            animationType = NPCID.Vulture;
            //First we get the NPC to Banner using the below.
            banner = Item.NPCtoBanner(NPCID.Zombie);
            //Then we link the banner to the banner item
            bannerItem = Item.BannerToItem(banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //This is a way of spawning an enemy.
            //SpawnCondition contains a few options and chance gives the chance
            //You can return any float
            return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
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
            Item.NewItem(npc.position, ItemID.DemoniteOre, 3);
            //This will make the NPC only drop this item 25% of the time
            if(Main.rand.Next(4) == 0)
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
