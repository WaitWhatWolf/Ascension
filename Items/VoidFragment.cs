using Ascension.Attributes;
using Ascension.Enums;
using Terraria;
using Terraria.ID;

namespace Ascension.Items
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
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
