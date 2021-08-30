using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/28 13:08:38")]
    public class StarPlatinum : Stand
    {
        public StarPlatinum(AscendedPlayer player) : base(player) { }

        public override string Name { get; } = "Star Platinum";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title)
                    + ", a stand which excels at\neverything outside of range.\nMost suited class: " + Hooks.Colors.GetColoredTooltipText("Melee", Hooks.Colors.Tooltip_Class)
                    + "\n\n"
                    + Hooks.Colors.GetColoredTooltipText("Good grief.", Hooks.Colors.Tooltip_Quote);

        public override StandID ID { get; } = StandID.STAR_PLATINUM;

        protected override int StandProjectileType => ModContent.ProjectileType<Projectile_StarPlatinum>();

        protected override StandAbility[] Init_Abilities => new StandAbility[]
            {
                new StandAbility_StarPlatinum_Punch(this),
                new StandAbility_StarPlatinum_ORA(this),
                new StandAbility_StarPlatinum_Receipt(this),
                new StandAbility_StarPlatinum_TheWorld(this),
            };

        protected override Asset<Texture2D> Init_Portrait => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_PORTRAIT_STARPLATINUM);

        protected override Stat base_stat_damage { get; } = new(5f, STATS_STACKING_BASE, Affection.Damage);
        protected override Stat base_stat_armorpen { get; } = new(10f, STATS_STACKING_BASE, Affection.ArmorPen);
        protected override Stat base_stat_attackspeed { get; } = new(180f, STATS_STACKING_BASE, Affection.AttackSpeed);
        protected override Stat base_stat_knockback { get; } = new(2f, STATS_STACKING_BASE, Affection.Knockback);
        protected override Stat base_stat_range { get; } = new(40f, STATS_STACKING_BASE, Affection.Range);
        protected override Stat base_stat_airange { get; } = new(200f, STATS_STACKING_BASE, Affection.MovementRange);
        protected override Stat base_stat_movespeed { get; } = new(20f, STATS_STACKING_BASE, Affection.MovementSpeed);

        protected override string StandInvokeSoundPath => "Stand_Invoke_StarPlatinum.wav";

        protected override void StatUpdater()
        {
            Owner.Player.GetDamage<MeleeDamageClass>() += (0.1f * Level);
            Owner.Player.meleeSpeed *= (1.1f * Level);
            Owner.Player.GetKnockback<MeleeDamageClass>() += (0.2f * Level);
            Owner.Player.GetCritChance<MeleeDamageClass>() += (5 * Level);
            Owner.Player.statDefense += (5 * Level);
        }
    }
}
