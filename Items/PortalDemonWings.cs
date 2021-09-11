using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Ascension.Enums;
using Ascension.Attributes;

namespace Ascension.Items
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Wings)]
    public class PortalDemonWings : AscensionItem
    {
        [Note(Dev.WaitWhatWolf, "You set the item's name as the tooltip and never set the name...@adi pls add something here it's your item xd")]
        protected override string TooltipDefault
        {
            get
            {
                Debug.LogWarning("@adi pls read note in PortalDemonWings");
                return "Ascension is further than those wings can reach";
            }
        }

        protected override int JourneyCheatCount { get; } = 1;

        //NotYetUsed
        /*public override bool Autoload(ref string name)
        {
            return !ModContent.GetInstance<ExampleConfigServer>().DisableExampleWings;
        }*/

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