using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Projectiles.Minions
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SilentMinionBuff : AscensionBuff
    {
        [ModifiedBy(Dev.WaitWhatWolf, "Changed sub-folder value from a raw string value to a constant in Ascension.ASCResources.", 2021, 08, 09)]
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, ASSETS_SUBPATH_MINIONS, this);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Silent Killer");
            Description.SetDefault("Baby Silent Killer minion, not really the brightest but it has a cool sword on the back");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

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
