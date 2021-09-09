using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Tiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/09 20:30:12")]
    public sealed class Tile_ParasiteSlime : AscensionTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			DustType = DustID.t_Slime;
			ItemDrop = ModContent.ItemType<Items.Placeables.Item_ParasiteSlimeBlock>();
			AddMapEntry(Color.DodgerBlue);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? DustID.t_Slime : DustID.Crimslime;
	}
}
