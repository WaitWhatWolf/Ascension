using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Ascension.Tiles
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    public class VoidAltar : AscensionTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 36,18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void Altar");
            AddMapEntry(new Color(200, 200, 200), name);           
            //disableSmartCursor = true;
            AdjTiles = new int[] { TileID.WorkBenches };
            MinPick = 100;
            Main.tileCut[Type] = false;
        }


        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.VoidAltarP>());
        }
    }
}
