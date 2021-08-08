using Ascension.Buffs;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Projectiles.Minions
{
    class SilentMinionBuff : AscensionBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Silent Killer");
            Description.SetDefault("Baby Silent Killer minion, not really the brightest but it has a cool sword on the back");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, "Minions/", this);

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SilentMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
