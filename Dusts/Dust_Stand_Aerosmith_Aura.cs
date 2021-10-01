using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;

namespace Ascension.Dusts
{
    /// <summary>
    /// Dust aura for Aerosmith.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/07 19:38:18")]
    public class Dust_Stand_Aerosmith_Aura : Dust_Stand_Aura
    {
        protected override Color StartColor => Color.Crimson;
        protected override Color EndColor => Hooks.Random.ChanceIn(2) ? Color.OrangeRed : Color.Silver;
    }
}
