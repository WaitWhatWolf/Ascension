using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Ascension.World
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    public class CompactWorldGen : ModSystem
    {
        // 3. We use the ModifyWorldGenTasks method to tell the game the order that our world generation code should run
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            // 4. We use FindIndex to locate the index of the vanilla world generation task called "Shinies". This ensures our code runs at the correct step.
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (ShiniesIndex != -1)
            {
                // 5. We register our world generation pass by passing in a name and the method that will execute our world generation code.	
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("VoidAltar.", WorldGenVoidAltar));
            }
        }

        // 6. This is the actual world generation code.
        private void WorldGenVoidAltar(GenerationProgress progress, GameConfiguration configuration)
        {
            // 7. Setting a progress message is always a good idea. This is the message the user sees during world generation and can be useful for identifying infinite loops.      
            progress.Message = "Placing the void altar";

            // 8. Here we use a for loop to run the code inside the loop many times. This for loop scales to the product of Main.maxTilesX, Main.maxTilesY, and 2E-05. 2E-05 is scientific notation and equal to 0.00002. Sometimes scientific notation is easier to read when dealing with a lot of zeros.
            // 9. In a small world, this math results in 4200 * 1200 * 0.00002, which is about 100. This means that we'll run the code inside the for loop 100 times. This is the amount Crimtane or Demonite will spawn. Since we are scaling by both dimensions of the world size, the ammount spawned will adjust automatically to different world sizes for a consistent distribution of ores.
            // 10. We randomly choose an x and y coordinate. The x coordinate is choosen from the far left to the far right coordinates. The y coordinate, however, is choosen from between WorldGen.worldSurfaceLow and the bottom of the map. We can use this technique to determine the depth that our ore should spawn at.
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);
            //int y = WorldGen.genRand.Next((int)WorldGen.worldSurface - 700, Main.maxTilesY);

            bool foundSurface = false;
            int y = 1;
            while (y < Main.worldSurface)
            {
                if (WorldGen.SolidTile(x, y))
                {
                    foundSurface = true;
                    break;
                }
                y++;
            }
            y = y - 50;

            Point point = new Point(x, y);
            Point point2 = new Point(x + 10, y);
            Point point3 = new Point(x - 10, y);
            Point point4 = new Point(x, y + 6);
            Point point5 = new Point(x - 10, y - 16);
            Point point6 = new Point(x + 10, y - 16);
            Point point7 = new Point(x, y - 4);
            Point point8 = new Point(x - 16, y - 18);
            Point point9 = new Point(x - 10, y - 6);
            Point point10 = new Point(x + 11, y - 6);
            // Code to test placed here:
            //WorldGen.TileRunner(x, y, WorldGen.genRand.Next(25, 30), WorldGen.genRand.Next(25, 30), mod.TileType("VoidTile"),true);
            //(ushort)ModContent.TileType<VoidTile>()

            //WorldGen.PlaceTile(point7.X, point7.Y, (ushort)ModContent.TileType<VoidAltar>());
            //WorldGen.PlaceObject(point7.X, point7.Y,mod.TileType("VoidAltar"));
            WorldUtils.Gen(point, new Shapes.Circle(12, 3), Actions.Chain(new GenAction[]
            {
              new Actions.SetTile((ushort)ModContent.TileType<VoidTile>()),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
            }));
            WorldUtils.Gen(point2, new Shapes.Circle(8, 5), Actions.Chain(new GenAction[]
            {
              new Actions.SetTile((ushort)ModContent.TileType<VoidTile>()),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
            }));
            WorldUtils.Gen(point3, new Shapes.Circle(8, 5), Actions.Chain(new GenAction[]
{
              new Actions.SetTile((ushort)ModContent.TileType<VoidTile>()),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            WorldUtils.Gen(point4, new Shapes.Circle(12, 5), Actions.Chain(new GenAction[]
{
              new Actions.SetTile((ushort)ModContent.TileType<VoidTile>()),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            WorldGen.PlaceObject(point7.X, point7.Y, ModContent.TileType<VoidAltar>());
            //column
            WorldUtils.Gen(point5, new Shapes.Rectangle(2, 14), Actions.Chain(new GenAction[]
{
              new Actions.PlaceWall(WallID.MarbleBlock),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            //column
            WorldUtils.Gen(point6, new Shapes.Rectangle(2, 14), Actions.Chain(new GenAction[]
{
              new Actions.PlaceWall(WallID.MarbleBlock),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            //Torch
            WorldUtils.Gen(point9, new Shapes.Rectangle(1, 4), Actions.Chain(new GenAction[]
{
              new Actions.PlaceTile(TileID.Lamps),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            WorldUtils.Gen(point10, new Shapes.Rectangle(1, 4), Actions.Chain(new GenAction[]
{
              new Actions.PlaceTile(TileID.Lamps),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
            //altar
            /*
            WorldUtils.Gen(point7, new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
{
              new Actions.SetTile(18),
              new Actions.PlaceTile((ushort)ModContent.TileType<VoidAltar>()),
              new Actions.SetFrames(false), 
              //new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));      
*/

            //roof
            WorldUtils.Gen(point8, new Shapes.Rectangle(34, 4), Actions.Chain(new GenAction[]
{
              new Actions.SetTile((ushort)ModContent.TileType<VoidTile>()),
              new Actions.Custom((i, j, args) => {Dust.QuickDust(new Point(i, j), Color.Purple); return true; }),
}));
        }
    }
}
