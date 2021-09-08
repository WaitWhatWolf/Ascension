using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Items.Rarities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Weapons
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/08 13:57:03")]
    public class Item_ParasiteSlimeSword : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Sword hits apply 2 stacks of parasite slime.";

        protected override string TextureSubFolder { get; } = ASCResources.ASSETS_SUBPATH_WEAPONS;

        protected override int JourneyCheatCount => 1;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 58;
            Item.scale = 1.25f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 45;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee; //Whether your item is part of the melee class.
            Item.damage = 5;
            Item.knockBack = 12;
            Item.crit = 6;

            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ModContent.RarityType<Rarity_Common>();
            Item.UseSound = SoundID.Item155;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            SoundEngine.PlaySound(SoundID.NPCHit1);
            ASCResources.Dusts.Dust_ParasiteSlime_Explode.Create(target.Center);

            if(ASCResources.Delegates.IsNotSlime(target))
                for(int i = 0; i < 2; i++)
                    target.AddBuff(ModContent.BuffType<Buff_Parasites>(), ASCResources.Trademark.PARASITESLIME_BUFF_DURATION);
        }

        public override bool? UseItem(Player player)
        {
            if(player.whoAmI == Main.myPlayer)
            {
                player.direction = Main.MouseWorld.X < player.position.X ? -1 : 1;
                return null;
            }
            return base.UseItem(player);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            ASCResources.Dusts.Dust_ParasiteSlime_ProjTravel(hitbox.Size()).Create(hitbox.TopLeft());
        }
    }
}
