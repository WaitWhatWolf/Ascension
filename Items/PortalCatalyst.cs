using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items
{
    public class PortalCatalyst : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Catalyst of Chaos");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 3);
            Item.maxStack = 10;
        }

    }
}
