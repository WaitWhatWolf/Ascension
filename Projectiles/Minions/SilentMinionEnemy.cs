using Ascension.Attributes;
using Ascension.Enums;
using Ascension.NPCs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Ascension.ASCResources;

namespace Ascension.Projectiles.Minions
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SilentMinionEnemy : AscensionNPC
    {
        [ModifiedBy(Dev.WaitWhatWolf, "Changed sub-folder value from a raw string value to a constant in Ascension.ASCResources.", 2021, 08, 09)]
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, ASSETS_SUBPATH_MINIONS, this);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Silent Killer");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 1; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)
        }

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 16;
            NPC.height = 16;
            //The Enemies max health
            NPC.lifeMax = 15;
            //Enemy Damage and Defence
            NPC.damage = 8;
            NPC.defense = 4;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath8;
            //The amount of money that is dropped (as a float?)
            NPC.value = 10f;
            //How much Knockback is resisted
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = 6;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.soundDelay = -1;
            NPC.behindTiles = false;
        }
    }
}