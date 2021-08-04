using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ascension
{
    public sealed class AscendedPlayer : ModPlayer
    {
        public float BaseUmbralCrit;

        /// <inheritdoc/>
        public override TagCompound Save()
        {
            return new TagCompound()
            {
                //["Stand"] = !Stand ? 0 : (int)Stand.ID
            };
        }

        /// <summary>
        /// Loads the stand.
        /// </summary>
        /// <param name="tag"></param>
        public override void Load(TagCompound tag)
        {
            //LoadedStandID = (StandType)tag.GetInt("Stand");
        }
    }
}
