using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items
{
    class VoidFragment : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Fragment");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Gray;
            Item.value = Item.sellPrice(silver: 1);
            Item.maxStack = 999;
        }
    }
}
