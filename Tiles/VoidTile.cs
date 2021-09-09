using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Tiles
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class VoidTile : AscensionTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = false;
            DustType = DustID.WhiteTorch;    
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void Tile");
            AddMapEntry(new Color(192, 192, 192), name);
            MinPick = 100;
            Main.tileCut[Type] = false;
        }

        /*
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.75f;
            b = 1f;
        }
        */

        public override bool Drop(int i, int j)
        {
            ModContent.ItemType<VoidBlock>();
            return true;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            Convert(i, j, 1);
        }


        public void Convert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && System.Math.Abs(k - i) + System.Math.Abs(l - j) < System.Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].type;
                        int wall = Main.tile[k, l].wall;

                        //If the tile is stone, convert to ExampleBlock
                        if (type != 0)
                        {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<VoidTile>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }
        
    }
}
