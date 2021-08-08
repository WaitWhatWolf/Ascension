using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Ascension.Items
{
    [AutoloadEquip(EquipType.Wings)]
    public class PortalDemonWings : AscensionItem
    {
        //NotYetUsed
        /*
        public override bool Autoload(ref string name)
        {
            return !ModContent.GetInstance<ExampleConfigServer>().DisableExampleWings;
        }
        */

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Portal Demon Wings");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        //these wings use the same values as the solar wings
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 120;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 0.15f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 2f;
            acceleration *= 1f;
        }
    }
}