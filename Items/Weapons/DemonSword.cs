using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Weapons
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class DemonSword : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Pierce the air!");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, "Weapons/", this);

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.noMelee = false;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 300;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false; //so the Item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<DemonSwordBurst>();
            Item.shootSpeed = 0f;
        }


        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item42, player.position);
            Projectile.NewProjectile(source,player.position.X + player.width / 2, player.position.Y + player.height / 2, 0, 0, ModContent.ProjectileType<DemonSwordBurst>(), Item.damage, (int)knockback, Main.myPlayer);
            return true;
        }

        public void MakeDustWeak()
        {
            int dust = Dust.NewDust(Item.position, Item.width + Main.rand.Next(-50, 51), Item.height + Main.rand.Next(-50, 51), DustID.PinkCrystalShard, 0f, 0f, 50, default(Color), 1f); //56 blue or 54 black
            Main.dust[dust].velocity *= 1.2f;
            Main.dust[dust].noGravity = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            MakeDustWeak();
        }
    }
}
