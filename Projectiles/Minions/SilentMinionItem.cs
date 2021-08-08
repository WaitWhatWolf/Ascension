using Ascension.Enums;
using Ascension.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Projectiles.Minions
{
    class SilentMinionItem : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silent Kill");
            Tooltip.SetDefault("Summons a baby silent killer to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, "Minions/", this);

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item44;

            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.buffType = ModContent.BuffType<SilentMinionBuff>();
            // No buffTime because otherwise the Item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<SilentMinion>();
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
            position = Main.MouseWorld;
            return true;
        }
    }
}
