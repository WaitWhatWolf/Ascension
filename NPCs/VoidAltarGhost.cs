using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ascension.Tiles;
using Ascension.Buffs;
using Ascension.Items;
using Ascension.Enums;
using Ascension.Attributes;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class VoidAltarGhost : AscensionNPC
    {
        Player player2; //used for adding a buff if the NPC is killed
        int counter; //used for framecounter
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Void Ghost");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 3; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)


            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { //Influences how the NPC looks in the Bestiary
                Velocity = 1f //Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 38;
            NPC.height = 46;
            //The Enemies max health
            NPC.lifeMax = 30;
            //Enemy Damage and Defence
            NPC.damage = 0;
            NPC.defense = 30;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            //The amount of money that is dropped (as a float?)
            NPC.value = 10f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = 22;
            NPC.noGravity = true;
            NPC.stepSpeed = 2;
            NPC.netAlways = true;
        }
        public override void AI()
        {
            counter++;
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(faceTarget: true);
            NPC.velocity = NPC.velocity * 0.9f;
        }
        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter++;
            if (counter > 0 && counter < 14)
            {
                    NPC.frame.Y = 0 * frameSize;
                    NPC.frameCounter = 0.0;
            }
            if (counter >= 15 && counter <29)
            {
                NPC.frame.Y = 1 * frameSize;
                NPC.frameCounter = 0.0;
            }
            if (counter >= 30 && counter <44)
            {
                NPC.frame.Y = 2 * frameSize;
                NPC.frameCounter = 0.0;
                counter = 0;
            }
            if (counter >= 45)
            {
                NPC.frameCounter = 0.0;
                counter = 0;
            }

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.spawnTileType == ModContent.TileType<VoidTile>() ? .6f : 0f;
        }
        /* //No current use of it
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Darkness, 240);
        }
        */ 

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            player2 = Main.player[projectile.owner];
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            player2 = player;
        }
        public override bool CheckDead()
        {
            if (base.CheckDead())
            {
                if (player2 == null)
                {
                    player2 = Main.player[NPC.target];
                }
                player2.AddBuff(ModContent.BuffType<ManaRegenVoid>(), 240, false);
                NPC.NPCLoot();
                return true;
            }

            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 1, 5));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This type of ghost appears near void blocks. Gives me mana to absorb after I slay one of them and is very weak overall.")
            });
        }
    }
}
