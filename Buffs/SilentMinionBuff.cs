using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Buffs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SilentMinionBuff : AscensionBuff
    {
        protected override string TextureSubFolder => ASSETS_SUBPATH_MINIONS;

        protected override string DisplayNameDefault { get; } = "Baby Silent Killer";
        protected override string DescriptionDefault { get; } = "Not really the brightest but it has a cool sword on the back";
        protected override bool SaveBuff { get; } = false;
        protected override bool DisplayBuffTimer { get; } = false;

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
