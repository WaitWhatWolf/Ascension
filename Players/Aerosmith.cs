using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/30 15:11:06")]
    public class Aerosmith : Stand
    {
        public Aerosmith(AscendedPlayer player) : base(player)
        {
        }

        public override string Name { get; } = nameof(Aerosmith);

        public override string Description { get; } = "{{c:Stand_Title=Aerosmith}} is a utility stand which focuses on " +
            "\ndealing constant damage rather than bursting down the opponent.";

        public override StandID ID { get; } = StandID.AEROSMITH;

        public override Color ThemeColor { get; } = Color.OrangeRed;

        public override DamageClass Class { get; } = DamageClass.Summon;

        public override string Quote { get; } = "There's no way in hell I'm gonna let you escape!";

        protected override int StandProjectileType => throw new NotImplementedException();

        protected override string StandInvokeSoundPath => throw new NotImplementedException();

        protected override StandAbility[] Init_Abilities => new StandAbility[]
        {
            new StandAbility_Aerosmith_Minigun(this, 0),
        };

        protected override Stat base_stat_damage { get; } = new Stat(5f, STATS_STACKING_BASE, Affection.Damage);
        protected override Stat base_stat_armorpen { get; } = new Stat(2f, STATS_STACKING_BASE, Affection.ArmorPen);
        protected override Stat base_stat_attackspeed { get; } = new Stat(1200f, STATS_STACKING_BASE, Affection.AttackSpeed);
        protected override Stat base_stat_knockback { get; } = new Stat(0.5f, STATS_STACKING_BASE, Affection.Knockback);
        protected override Stat base_stat_range { get; } = new Stat(120f, STATS_STACKING_BASE, Affection.Range);
        protected override Stat base_stat_airange { get; } = new Stat(360f, STATS_STACKING_BASE, Affection.MovementRange);
        protected override Stat base_stat_movespeed { get; } = new Stat(10f, STATS_STACKING_BASE, Affection.MovementSpeed);

        protected override Asset<Texture2D> Init_Portrait => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_PORTRAIT_AEROSMITH);

        protected override void StatUpdater()
        {
            Owner.Player.GetDamage<SummonDamageClass>() += (0.15f * Level);
            Owner.Player.GetKnockback<SummonDamageClass>() += (0.25f * Level);
            Owner.Player.GetCritChance<SummonDamageClass>() += (7 * Level);
        }
    }
}
