using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;

namespace Ascension.Dusts
{
    /// <summary>
    /// Dust aura for killer queen.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/07 19:38:18")]
    public class Dust_Stand_KillerQueen_Aura : Dust_Stand_Aura
    {
        protected override Color StartColor => Color.Magenta;
        protected override Color EndColor => Hooks.Random.ChanceIn(2) ? Color.DeepPink : Color.DodgerBlue;
    }
}
