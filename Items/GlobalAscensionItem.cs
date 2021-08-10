using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class GlobalAscensionItem : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (arg == ItemID.KingSlimeBossBag)
            {
                Item.NewItem(player.position, ModContent.ItemType<Item_WormedSlimeSample>(), Hooks.Random.Range(3, 5), noGrabDelay: true);
            }
        }
    }
}
