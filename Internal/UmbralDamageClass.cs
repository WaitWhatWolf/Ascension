using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Terraria.ModLoader;

namespace Ascension.Internal
{
    /// <summary>
    /// <see cref="DamageClass"/> type for the Umbral class.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
    public sealed class UmbralDamageClass : DamageClass
    {
        public override void SetStaticDefaults()
        {
            foreach(var player in Mod.GetContent<AscendedPlayer>())
                player.BaseUmbralCrit = ASCResources.Stats.UMBRAL_CRIT_BASE;
        }

        /// <summary>
        /// The <see cref="UmbralDamageClass"/> benefits from <see cref="ThrowingDamageClass"/>.
        /// </summary>
        /// <inheritdoc/>
        protected override float GetBenefitFrom(DamageClass damageClass)
        {
            return CountsAs(damageClass) ? 1f : 0f;
        }

        /// <summary>
        /// The <see cref="UmbralDamageClass"/> also counts as <see cref="ThrowingDamageClass"/>.
        /// </summary>
        /// <param name="damageClass"></param>
        /// <returns></returns>
        public override bool CountsAs(DamageClass damageClass)
        {
            return damageClass.FullName == Throwing.FullName;
        }
    }
}
