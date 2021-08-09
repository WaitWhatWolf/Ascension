using Ascension.Attributes;
using Ascension.Enums;
using Terraria;
using Terraria.ID;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class ProphetSwordOccultist : AscensionNPC
    {
        public override void SetStaticDefaults()
        {
            //The name the enemy displays
            DisplayName.SetDefault("Prophet of Sword Occult");

            //The frame count for the enemy
            Main.npcFrameCount[NPC.type] = 3; //Zombie has 3 frames but u can instead type in number 3 instead (depending how many frames you want)
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.NPCs, "ProphetSwordOccultist/", this);

        public override void SetDefaults()
        {
            //Enemy Hitbox Width and Height
            NPC.width = 38;
            NPC.height = 46;
            //The Enemies max health
            NPC.lifeMax = 10000;
            //Enemy Damage and Defence
            NPC.damage = 50;
            NPC.defense = 12;
            //The sound the enemy makes upon hit or death
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            //The amount of money that is dropped (as a float?)
            NPC.value = 1000f;
            NPC.aiStyle = -1;
            //mod.ProjectileType("SmallPurpleFlame");
            NPC.boss = true;
            NPC.knockBackResist = 0.01f;
            NPC.aiStyle = 0;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.behindTiles = false;
            //Music = ModContent.GetSoundSlot(SoundType.Music, "Sounds/Music/POEHEISTWAV");
        }
    }
}
