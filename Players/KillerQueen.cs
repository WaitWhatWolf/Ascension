using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/28 13:22:02")]
    public class KillerQueen : Stand
    {
        public KillerQueen(AscendedPlayer player) : base(player) { }
        
        public override string Name { get; } = "Killer Queen";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Killer Queen", Hooks.Colors.Tooltip_Stand_Title)
                    + " is a stand which focuses primarily on the offensive,\nusing utility for defense rather than tough skin."
                    + "\nMost suited class: " + Hooks.Colors.GetColoredTooltipText("Melee", Hooks.Colors.Tooltip_Class)
                    + "\n\n"
                    + Hooks.Colors.GetColoredTooltipMultilineText("My name is Yoshikage Kira. I'm 33 years old."
                    + "\nMy house is in the northeast section of Morioh,"
                    + "\nwhere all the villas are, and I am not married."
                    + "\nI work as an employee for the Kame Yu department stores,"
                    + "\nand I get home every day by 8 PM at the latest."
                    + "\nI don't smoke, but I occasionally drink."
                    + "\nI'm in bed by 11 PM, and make sure I get eight hours of sleep, no matter what."
                    + "\nAfter having a glass of warm milk"
                    + "\nand doing about twenty minutes of stretches before going to bed,"
                    + "\nI usually have no problems sleeping until morning."
                    + "\nJust like a baby, I wake up without any fatigue or stress in the morning."
                    + "\nI was told there were no issues at my last check-up."
                    + "\nI'm trying to explain that I'm a person who wishes to live a very quiet life."
                    + "\nI take care not to trouble myself with any enemies,"
                    + "\nlike winning and losing, that would cause me to lose sleep at night."
                    + "\nThat is how I deal with society, and I know that is what brings me happiness."
                    + "\nAlthough, if I were to fight I wouldn't lose to anyone.", Hooks.Colors.Tooltip_Quote);

        public override StandID ID { get; } = StandID.KILLER_QUEEN;

        protected override int StandProjectileType => ModContent.ProjectileType<Projectile_KillerQueen>();

        protected override StandAbility[] Init_Abilities => new StandAbility[]
        {
            new StandAbility_KillerQueen_BombTransmutation(this, 0),
            new StandAbility_KillerQueen_StrayCatBombing(this, 1),
            new StandAbility_KillerQueen_SheerHeartAttack(this, 2),
            new StandAbility_KillerQueen_BitesTheDust(this, 3),
        };

        protected override Asset<Texture2D> Init_Portrait => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_PORTRAIT_KILLERQUEEN);

        protected override Stat base_stat_damage { get; } = new(8f, STATS_STACKING_BASE, Affection.Damage);
        protected override Stat base_stat_armorpen { get; } = new(20f, STATS_STACKING_BASE, Affection.ArmorPen);
        protected override Stat base_stat_attackspeed { get; } = new(30f, STATS_STACKING_BASE, Affection.AttackSpeed);
        protected override Stat base_stat_knockback { get; } = new(5f, STATS_STACKING_BASE, Affection.Knockback);
        protected override Stat base_stat_range { get; } = new(40f, STATS_STACKING_BASE, Affection.Range);
        protected override Stat base_stat_airange { get; } = new(500f, STATS_STACKING_BASE, Affection.MovementRange);
        protected override Stat base_stat_movespeed { get; } = new(10f, STATS_STACKING_BASE, Affection.MovementSpeed);
        protected override KeyValuePair<string, Stat>[] base_stats_other => new KeyValuePair<string, Stat>[]
        {
            new KeyValuePair<string, Stat>(STAND_STAT_PROJECTILE_VELOCITY, new Stat(5f, STATS_STACKING_BASE, Affection.ProjectileSpeed))
        };

        protected override string StandInvokeSoundPath => "Stand_Invoke_KillerQueen.wav";

        protected override void StatUpdater()
        {
            Owner.Player.GetDamage<MeleeDamageClass>() += (0.15f * Level);
            Owner.Player.GetKnockback<MeleeDamageClass>() += (0.25f * Level);
            Owner.Player.GetCritChance<MeleeDamageClass>() += (7 * Level);
            if (Abilities[2].Active) //Reduces swing speed while Sheer Heart Attack is active
            {
                Owner.Player.meleeSpeed *= 0.5f;
                Owner.Player.pickSpeed *= 0.5f;
            }
            Owner.Player.statDefense -= (2 * Level);
        }
    }
}
