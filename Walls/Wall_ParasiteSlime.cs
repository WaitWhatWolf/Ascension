using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Walls
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/10 11:14:51")]
    public sealed class Wall_ParasiteSlime : AscensionWall
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.t_Slime;
            ItemDrop = ModContent.ItemType<Items.Placeables.Item_ParasiteSlimeWall>();
            AddMapEntry(Color.MediumSlateBlue);
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 2;
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = (int)(Hooks.Random.ChanceIn(2) ? DustType : DustID.Crimslime);

            return false;
        }
    }
}
