using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Items.Rarities;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class Item_WormedSlimeJavelin : AscensionItem
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, ASCResources.ASSETS_SUBPATH_WEAPONS, this);
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wormed Slime Javelin");
            Tooltip.SetDefault("Throws Slime Javelins which explode on impact.\nExplosions create worms that lach onto enemies.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Javelin);

            Item.height = 35;
            Item.width = 35;
            Item.DamageType = ModContent.GetInstance<UmbralDamageClass>();
            Item.damage = 12;
            Item.consumable = false;
            Item.maxStack = 1;

            Item.rare = ModContent.RarityType<Rarity_Common>();
        }
    }
}
